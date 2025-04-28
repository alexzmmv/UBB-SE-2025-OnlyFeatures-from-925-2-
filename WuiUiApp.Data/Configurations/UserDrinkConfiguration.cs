﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WuiUiApp.Data.Data;

namespace WuiUiApp.Data.Configurations
{
    public class UserDrinkConfiguration : IEntityTypeConfiguration<UserDrink>
    {
        public void Configure(EntityTypeBuilder<UserDrink> builder)
        {
            builder.HasKey(userDrink => new { userDrink.UserId, userDrink.DrinkId });

            builder.HasOne(userDrink => userDrink.User)
                   .WithMany()
                   .HasForeignKey(user => user.UserId);

            builder.HasOne(userDrink => userDrink.Drink)
                   .WithMany()
                   .HasForeignKey(drink => drink.DrinkId);
        }
    }
}
