using WinUiApp.Data.Data;
using WinUIApp.WebAPI.Models;

public static class ReviewExtensions
{
    public static WinUiApp.Data.Data.Review ToDataModel(this WinUIApp.WebAPI.Models.Review review) =>
        new WinUiApp.Data.Data.Review 
        { 
            ReviewId = review.ReviewId,
            RatingId = review.RatingId,
            UserId = review.UserId,
            Content = review.Content,
            CreationDate = review.CreationDate,
            IsActive = review.IsActive
        };
}