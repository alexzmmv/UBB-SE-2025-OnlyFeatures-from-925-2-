using WinUiApp.Data.Data;

namespace WinUIApp.WebAPI.Models;

public static class RatingExtensions
{
    public static WinUiApp.Data.Data.Rating ToDataModel(this Rating rating) => 
        new WinUiApp.Data.Data.Rating
        {
            RatingDate = rating.RatingDate,
            RatingValue = rating.RatingValue,
            UserId = rating.UserId,
            DrinkId = rating.DrinkId,
            IsActive = rating.IsActive,
        };
    public static Rating ToModel(this WinUiApp.Data.Data.Rating rating) => 
        new Rating
        {
            RatingId   = rating.RatingId,
            RatingDate = rating.RatingDate ?? new DateTime(),
            RatingValue = rating.RatingValue ?? 0,
            UserId = rating.UserId,
            DrinkId = rating.DrinkId,
            IsActive = rating.IsActive ?? false,
        };

    public static IEnumerable<Rating> ToModels(this IEnumerable<WinUiApp.Data.Data.Rating> ratings)
    {
        return ratings.Select(rating => rating.ToModel());
    }
}
