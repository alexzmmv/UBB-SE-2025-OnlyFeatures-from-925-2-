using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WinUIApp.ProxyServices;
using WinUIApp.WebMVC.Models;

namespace WinUIApp.WebMVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    
    private readonly IDrinkService drinkService;

    public HomeController(ILogger<HomeController> logger,IDrinkService drinkService)
    {
        this.drinkService = drinkService;
    }

    public IActionResult Index()
    {
        var drinkOfTheDay = drinkService.GetDrinkOfTheDay();
        var drinkCategories = drinkService.GetDrinkCategories();
        var drinkBrands = drinkService.GetDrinkBrandNames();

        var homeViewModel = new HomeViewModel
        {
            DrinkOfTheDay = drinkOfTheDay,
            drinkCategories = drinkCategories,
            drinkBrands = drinkBrands,
        };
        
        return View(homeViewModel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
