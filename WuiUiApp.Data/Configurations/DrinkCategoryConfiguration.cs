using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WuiUiApp.Data.Data;

namespace WuiUiApp.Data.Configurations
{
    public class DrinkCategoryConfiguration : IEntityTypeConfiguration<DrinkCategory>
    {
        public void Configure(EntityTypeBuilder<DrinkCategory> builder)
        {
            builder.HasKey(drinkCategory => new { drinkCategory.DrinkId, drinkCategory.CategoryId });

            builder.HasOne(drinkCategory => drinkCategory.Drink)
                   .WithMany()
                   .HasForeignKey(drinkCategory => drinkCategory.DrinkId);

            builder.HasOne(drinkCategory => drinkCategory.Category)
                   .WithMany()
                   .HasForeignKey(drinkCategory => drinkCategory.CategoryId);
        }
    }
}
