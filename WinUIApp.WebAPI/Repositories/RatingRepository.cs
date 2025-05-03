// <copyright file="RatingRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("WinUIApp.Tests")]

namespace WinUIApp.WebAPI.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using WinUiApp.Data;
    using WinUiApp.Data.Data;

    /// <summary>
    /// Repository for managing rating-related operations.
    /// </summary>
    internal class RatingRepository : IRatingRepository
    {
        private readonly AppDbContext dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="RatingRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public RatingRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <inheritdoc/>
        public Rating? GetRatingById(int ratingId)
        {
            return this.dbContext.Ratings
                .FirstOrDefault(r => r.RatingId == ratingId);
        }

        /// <inheritdoc/>
        public List<Rating> GetAllRatings()
        {
            return this.dbContext.Ratings.ToList();
        }

        /// <inheritdoc/>
        public List<Rating> GetRatingsByDrinkId(int drinkId)
        {
            return this.dbContext.Ratings
                .Where(r => r.DrinkId == drinkId)
                .ToList();
        }

        /// <inheritdoc/>
        public void AddRating(Rating rating)
        {
            rating.RatingDate ??= DateTime.UtcNow;
            rating.IsActive ??= true;

            this.dbContext.Ratings.Add(rating);
            this.dbContext.SaveChanges();
        }

        /// <inheritdoc/>
        public void UpdateRating(Rating rating)
        {
            var existingRating = this.dbContext.Ratings
                .FirstOrDefault(r => r.RatingId == rating.RatingId);

            if (existingRating == null)
            {
                throw new Exception("No rating found with the provided Id.");
            }

            existingRating.DrinkId = rating.DrinkId;
            existingRating.UserId = rating.UserId;
            existingRating.RatingValue = rating.RatingValue;
            existingRating.RatingDate = rating.RatingDate ?? existingRating.RatingDate;
            existingRating.IsActive = rating.IsActive ?? existingRating.IsActive;

            this.dbContext.SaveChanges();
        }

        /// <inheritdoc/>
        public void DeleteRating(int ratingId)
        {
            var rating = this.dbContext.Ratings
                .FirstOrDefault(r => r.RatingId == ratingId);

            if (rating == null)
            {
                throw new Exception("No rating found with the provided Id.");
            }

            this.dbContext.Ratings.Remove(rating);
            this.dbContext.SaveChanges();
        }
    }
}
