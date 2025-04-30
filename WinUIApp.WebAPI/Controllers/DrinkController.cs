using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WinUIApp.Models;
using WinUIApp.Services;

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


        [HttpGet("get-one")]
        public IActionResult GetDrinkById([FromQuery] int drinkId)
        {
            return Ok(_drinkService.GetDrinkById(drinkId));
        }
    }
}
