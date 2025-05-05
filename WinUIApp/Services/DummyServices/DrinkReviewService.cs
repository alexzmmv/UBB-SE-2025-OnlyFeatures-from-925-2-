// <copyright file="DrinkReviewService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.Services.DummyServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using WinUIApp.Models;

    /// <summary>
    /// Service for managing drink reviews.
    /// </summary>
    public class DrinkReviewService : IDrinkReviewService
    {

        /// <summary>
        /// Retrieves all reviews for a specific drink by its ID.
        /// </summary>
        /// <param name="drinkID"> Drink id. </param>
        /// <returns> Reviews for the drink. </returns>
        public List<Review> GetReviewsByDrinkID(int drinkID)
        {
            return [];
        }

        /// <summary>
        /// Calculates the average review score for a drink by its ID.
        /// </summary>
        /// <param name="drinkID"> Drink id. </param>
        /// <returns> Average review. </returns>
        public float GetReviewAverageByDrinkID(int drinkID)
        {
            return 0.0f;
        }
    }
}