using Drink = WinUIApp.ProxyServices.Models.Drink;

namespace WinUIApp.WebMVC.Models;

public class FavoriteDrinksViewModel
{
    public List<Drink> FavoriteDrinks { get; set; } = new();
} 