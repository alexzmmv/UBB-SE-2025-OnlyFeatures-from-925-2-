using WinUIApp.ProxyServices.Models;
using Drink = WinUIApp.ProxyServices.Models.Drink;

namespace WinUIApp.WebMVC.Models;

public class HomeViewModel
{
    public Drink DrinkOfTheDay { get; set; }
    public List<Category> drinkCategories { get; set; }
    public List<Brand> drinkBrands { get; set; }
    public List<int> SelectedCategoryIds { get; set; }
    public List<int> SelectedBrandIds { get; set; }
}