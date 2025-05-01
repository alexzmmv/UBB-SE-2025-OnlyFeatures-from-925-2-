using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WinUIApp.Models;
using WinUIApp.Services;
using WinUIApp.WebAPI.Requests.Drink;

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


        [HttpGet("get-all")]
        public IActionResult GetAllDrinks([FromBody] GetDrinksRequest request)
        {
            return Ok();
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
        public IActionResult GetDrinkBrandNames() {
            return Ok(_drinkService.GetDrinkCategories());
        }

        [HttpGet("get-drink-of-the-day")]
        public IActionResult GetDrinkOfTheDay()
        {
            return Ok(_drinkService.GetDrinkOfTheDay());
        }

        [HttpGet("get-user-drink-list")]
        public IActionResult GetUserPersonalDrinkList([FromBody] GetUserDrinkListRequest request)
        {
            return Ok(_drinkService.GetUserPersonalDrinkList(request.userId));
        }

        [HttpPost("add")]
        public IActionResult AddDrink([FromBody] AddDrinkRequest request)
        {
            return Ok();
        }

        [HttpPost("vote-drink-of-the-day")]
        public IActionResult VoteDrinkOfTheDay()
        {
            return Ok();
        }

        [HttpPut("update")]
        public IActionResult UpdateDrink([FromBody] UpdateDrinkRequest request)
        {
            return Ok();
        }

        [HttpDelete("delete")]
        public IActionResult DeleteDrink([FromBody] DeleteDrinkRequest request)
        {
            _drinkService.DeleteDrink(request.drinkId);
            return Ok();
        }
    }
}
