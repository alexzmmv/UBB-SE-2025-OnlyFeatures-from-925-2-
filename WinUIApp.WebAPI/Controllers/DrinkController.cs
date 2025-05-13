using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WinUIApp.WebAPI.Models;
using WinUIApp.WebAPI.Requests.Drink;
using WinUIApp.WebAPI.Services;

namespace WinUIApp.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DrinkController : ControllerBase
    {
        IDrinkService _drinkService;

        public DrinkController(IDrinkService drinkService)
        {
            _drinkService = drinkService;
        }

        [HttpPost("get-all")]
        public IActionResult GetAllDrinks([FromBody] GetDrinksRequest request)
        {
            return Ok(
                _drinkService.GetDrinks(
                    request.searchKeyword,
                    request.drinkBrandNameFilter,
                    request.drinkCategoryFilter,
                    request.minimumAlcoholPercentage,
                    request.maximumAlcoholPercentage,
                    request.orderingCriteria));
        }
        
        [HttpGet("get-one")]
        public IActionResult GetDrinkById([FromQuery] int drinkId)
        {
            return Ok(_drinkService.GetDrinkById(drinkId));
        }

        [HttpGet("get-drink-brands")]
        public IActionResult GetDrinkBrands()
        {
            return Ok(_drinkService.GetDrinkBrandNames());
        }

        [HttpGet("get-drink-categories")]
        public IActionResult GetDrinkCategories() {
            return Ok(_drinkService.GetDrinkCategories());
        }

        [HttpGet("get-drink-of-the-day")]
        public IActionResult GetDrinkOfTheDay()
        {
            return Ok(_drinkService.GetDrinkOfTheDay());
        }

        [HttpPost("get-user-drink-list")]
        public IActionResult GetUserPersonalDrinkList([FromBody] GetUserDrinkListRequest request)
        {
            return Ok(_drinkService.GetUserPersonalDrinkList(request.userId));
        }
        
        [HttpPost("add")]
        public IActionResult AddDrink([FromBody] AddDrinkRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            _drinkService.AddDrink(
                request.inputtedDrinkName,
                request.inputtedDrinkPath,
                request.inputtedDrinkCategories,
                request.inputtedDrinkBrandName,
                request.inputtedAlcoholPercentage);
            return Ok();
        }

        [HttpPost("add-to-user-drink-list")]
        public IActionResult AddToUserPersonalDrinkList([FromBody] AddToUserPersonalDrinkListRequest request)
        {
            return Ok(_drinkService.AddToUserPersonalDrinkList(request.userId, request.drinkId));
        }

        [HttpPost("vote-drink-of-the-day")]
        public IActionResult VoteDrinkOfTheDay(VoteDrinkOfTheDayRequest request)
        {
            return Ok(_drinkService.VoteDrinkOfTheDay(request.userId,request.drinkId));
        }

        [HttpPut("update")]
        public IActionResult UpdateDrink([FromBody] UpdateDrinkRequest request)
        {
            _drinkService.UpdateDrink(request.drink);
            return Ok();
        }

        [HttpDelete("delete")]
        public IActionResult DeleteDrink([FromBody] DeleteDrinkRequest request)
        {
            _drinkService.DeleteDrink(request.drinkId);
            return Ok();
        }

        [HttpDelete("delete-from-user-drink-list")]
        public IActionResult DeleteFromUserPersonalDrinkList([FromBody] DeleteFromUserPersonalDrinkListRequest request)
        {
            return Ok(_drinkService.DeleteFromUserPersonalDrinkList(request.userId, request.drinkId));
        }
    }
}
