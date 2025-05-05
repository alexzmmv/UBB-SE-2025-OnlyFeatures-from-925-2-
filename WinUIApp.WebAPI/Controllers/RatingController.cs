
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WinUIApp.WebAPI.Models;
using WinUIApp.WebAPI.Requests.Rating;
using WinUIApp.WebAPI.Services;

namespace WinUIApp.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RatingController : ControllerBase
    {
        IRatingService _ratingService;

        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }
        
        [HttpGet("get-one")]
        public IActionResult GetRatingById([FromQuery] int ratingId)
        {
            return Ok(_ratingService.GetRatingById(ratingId));
        }
        
        [HttpGet("get-ratings-by-drink")]
        public IActionResult GetRatingsByDrink([FromQuery] int drinkId)
        {
            return Ok(_ratingService.GetRatingsByDrink(drinkId));
        }
        
        [HttpGet("get-average-rating-by-drink")]
        public IActionResult GetAverageRating([FromQuery] int drinkId)
        {
            return Ok(_ratingService.GetAverageRating(drinkId));
        }
            
        [HttpPost("add")]
        public IActionResult AddRating([FromBody] AddRatingRequest request)
        {
            _ratingService.CreateRating(request.rating);
            
            return Ok();
        }
        
        [HttpPut("update")]
        public IActionResult UpdateRating([FromBody] UpdateRatingRequest request)
        {
            _ratingService.UpdateRating(request.rating);
            
            return Ok();
        }
        
        [HttpDelete("delete")]
        public IActionResult DeleteRating([FromQuery] int ratingId)
        {
            _ratingService.DeleteRatingById(ratingId);
            
            return Ok();
        }
    }
}
