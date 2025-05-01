namespace WinUIApp.WebAPI.Requests.Drink
{
    public class GetDrinksRequest
    {
        string? searchKeyword { get; set; }
        List<string>? drinkBrandNameFilter { get; set; }
        List<string>? drinkCategoryFilter { get; set; }
        float? minimumAlcoholPercentage { get; set; }
        float? maximumAlcoholPercentage { get; set; }
        Dictionary<string, bool>? orderingCriteria { get; set; }
    }
}
