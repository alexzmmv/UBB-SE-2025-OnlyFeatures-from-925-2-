using Microsoft.AspNetCore.Mvc;
using WinUIApp.WebAPI.Services;
using System;
using WinUIApp.WebAPI.Models;

namespace WinUIApp.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet("get-by-rating")]
        public IActionResult GetReviewsByRating([FromQuery] int ratingId)
        {
            var reviews = _reviewService.GetReviewsByRating(ratingId);
            return Ok(reviews);
        }

        [HttpPost("add")]
        public IActionResult AddReview([FromBody] Review review)
        {
            ArgumentNullException.ThrowIfNull(review);

            try
            {
                var addedReview = _reviewService.AddReview(review);
                return Ok(addedReview);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete")]
        public IActionResult DeleteReview([FromQuery] int reviewId)
        {
            _reviewService.DeleteReviewById(reviewId);
            return Ok();
        }
    }
}
