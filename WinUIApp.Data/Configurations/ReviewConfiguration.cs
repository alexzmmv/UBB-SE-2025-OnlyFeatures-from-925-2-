using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WinUiApp.Data.Data;

namespace WinUiApp.Data.Configurations
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.HasKey(review => review.ReviewID);

            builder.Property(review => review.ReviewID)
                   .ValueGeneratedOnAdd();

            builder.Property(review => review.RatingID)
                   .IsRequired();

            builder.Property(review => review.UserID)
                   .IsRequired(false);

            builder.Property(review => review.Content)
                   .HasColumnType("text");

            builder.Property(review => review.CreationDate)
                   .HasColumnType("datetime");

            builder.Property(review => review.IsActive)
                   .HasColumnType("tinyint")
                   .HasDefaultValue(null);
                   
            builder.HasOne(review => review.Rating)
                   .WithMany()
                   .HasForeignKey(review => review.RatingID)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}