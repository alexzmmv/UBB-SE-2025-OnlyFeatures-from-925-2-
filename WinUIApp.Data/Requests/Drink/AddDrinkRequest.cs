using WinUiApp.Data.Data;

namespace WinUIApp.Data.Requests.Drink
{
    public class AddDrinkRequest
    {
        public string inputtedDrinkName { get; set; }
        public string inputtedDrinkPath { get; set; }
        public List<Category> inputtedDrinkCategories { get; set; }
        public string inputtedDrinkBrandName { get; set; }
        public float inputtedAlcoholPercentage { get; set; }
    }
}
