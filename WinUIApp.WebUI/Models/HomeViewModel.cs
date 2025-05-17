using Drink = WinUIApp.ProxyServices.Models.Drink;

namespace WinUIApp.WebMVC.Models;

public class HomeViewModel
{
    public Drink DrinkOfTheDay { get; set; }
}