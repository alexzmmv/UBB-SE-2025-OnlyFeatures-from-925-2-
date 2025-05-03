using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WinUiApp.Data.Data;

namespace WinUiApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Brand> Brands { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Drink> Drinks { get; set; }
    public DbSet<DrinkCategory> DrinkCategories { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Vote> Votes { get; set; }
    public DbSet<DrinkOfTheDay> DrinkOfTheDays { get; set; }
    public DbSet<UserDrink> UserDrinks { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Rating> Ratings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    private void ApplyConfigurationsFromAssembly(ModelBuilder modelBuilder)
    {
        var assembly = Assembly.GetExecutingAssembly();

        var entityTypeConfigurations = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract)
            .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)))
            .ToList();

        foreach (var config in entityTypeConfigurations)
        {
            dynamic instance = Activator.CreateInstance(config);
            modelBuilder.ApplyConfiguration(instance);
        }
    }

}