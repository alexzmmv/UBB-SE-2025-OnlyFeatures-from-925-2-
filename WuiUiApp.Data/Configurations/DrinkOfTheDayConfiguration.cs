using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WuiUiApp.Data.Data;

namespace WuiUiApp.Data.Configurations
{
    public class DrinkOfTheDayConfiguration : IEntityTypeConfiguration<DrinkOfTheDay>
    {
        public void Configure(EntityTypeBuilder<DrinkOfTheDay> builder)
        {
            builder.HasKey(drink => drink.DrinkId);

            builder.HasOne(drink => drink.Drink)
                   .WithMany()
                   .HasForeignKey(drink => drink.DrinkId);
        }
    }
}
