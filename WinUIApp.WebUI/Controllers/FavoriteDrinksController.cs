using Microsoft.AspNetCore.Mvc;
using WinUIApp.ProxyServices;
using WinUIApp.WebMVC.Models;

namespace WinUIApp.WebMVC.Controllers;

public class FavoriteDrinksController : Controller
{
    private readonly IDrinkService drinkService;

    public FavoriteDrinksController(IDrinkService drinkService)
    {
        this.drinkService = drinkService;
    }

    public IActionResult FavoriteDrinks()
    {
        const int CurrentUserId = 1;

        var favoriteDrinks = this.drinkService.GetUserPersonalDrinkList(CurrentUserId);
        
        var viewModel = new FavoriteDrinksViewModel
        {
            FavoriteDrinks = favoriteDrinks
        };
        
        return View(viewModel);
    }
} 