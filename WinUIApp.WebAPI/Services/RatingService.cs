// <copyright file="RatingService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.WebAPI.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using WinUiApp.Data.Data;
    using WinUIApp.WebAPI.Constants.ErrorMessages;
    using WinUIApp.WebAPI.Extensions;
    using WinUIApp.WebAPI.Repositories;

    /// <summary>
    /// Implementation of the rating service.
    /// </summary>
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository ratingRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="RatingService"/> class.
        /// </summary>
        /// <param name="ratingRepository">The rating repository dependency.</param>
        public RatingService(IRatingRepository ratingRepository)
        {
            this.ratingRepository = ratingRepository;
        }

        /// <summary>
        /// Retrieves a rating by its unique identifier.
        /// </summary>
        /// <param name="ratingId">The rating identifier.</param>
        /// <returns>The corresponding rating or null if it doesnt exist.<see cref="Rating"/>.</returns>
        public Rating? GetRatingById(int ratingId) => this.ratingRepository.GetRatingById(ratingId);

        /// <summary>
        /// Retrieves all ratings associated with a specific drink.
        /// </summary>
        /// <param name="drinkId">The drink identifier.</param>
        /// <returns>A collection of <see cref="Rating"/> instances for the drink.</returns>
        public IEnumerable<Rating> GetRatingsByDrink(int drinkId) => this.ratingRepository.GetRatingsByDrinkId(drinkId);

        /// <summary>
        /// Creates a new rating.
        /// </summary>
        /// <param name="rating">The rating to create.</param>
        /// <returns>The created <see cref="Rating"/> instance.</returns>
        /// <exception cref="System.ArgumentException">Thrown when the rating is invalid.</exception>
        public Rating CreateRating(Rating rating)
        {
            if (!rating.IsValid())
            {
                throw new ArgumentException(ServiceErrorMessages.InvalidRatingValue);
            }

            return this.ratingRepository.AddRating(rating);
        }

        /// <summary>
        /// Updates an existing rating.
        /// </summary>
        /// <param name="rating">The rating to update.</param>
        /// <returns>The updated <see cref="Rating"/> instance.</returns>
        /// <exception cref="System.ArgumentException">Thrown when the rating is invalid.</exception>
        public Rating UpdateRating(Rating rating)
        {
            if (!rating.IsValid())
            {
                throw new ArgumentException(ServiceErrorMessages.InvalidRatingValue);
            }

            return this.ratingRepository.UpdateRating(rating);
        }

        /// <summary>
        /// Deletes a rating by its unique identifier.
        /// </summary>
        /// <param name="ratingId">The rating identifier.</param>
        public void DeleteRatingById(int ratingId) => this.ratingRepository.DeleteRating(ratingId);


        private const double NoRatingsValue = 0.0;
        /// <summary>
        /// Calculates the average value of all ratings for a drink.
        /// </summary>
        /// <param name="drinkId">The drink identifier.</param>
        /// <returns>The average rating value, or 0 if no ratings are present.</returns>
        public double GetAverageRating(int drinkId)
        {
            var allRatings = this.ratingRepository.GetRatingsByDrinkId(drinkId).ToList();

            var validRatings = allRatings
                .Where(rating => rating.RatingValue.HasValue)
                .Select(rating => rating.RatingValue.Value)
                .ToList();

            if (!validRatings.Any())
            {
                return NoRatingsValue;
            }

            return (double)validRatings.Average();
        }
    }
}
