﻿// <copyright file="DrinkDisplayItem.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.Views.Components.SearchPageComponents
{
    using System;
    using WinUIApp.Models;

    /// <summary>
    /// Represents a drink item to display on the search page, along with its average review score.
    /// </summary>
    public class DrinkDisplayItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DrinkDisplayItem"/> class.
        /// </summary>
        /// <param name="drink">The drink to display.</param>
        /// <param name="averageReviewScore">The average review score.</param>
        /// <exception cref="ArgumentNullException">Thrown when drink is null.</exception>
        public DrinkDisplayItem(Drink drink, float averageReviewScore)
        {
            this.Drink = drink ?? throw new ArgumentNullException(nameof(drink), "Drink cannot be null.");
            this.AverageReviewScore = averageReviewScore;
        }

        /// <summary>
        /// Gets the drink being displayed.
        /// </summary>
        public Drink Drink { get; }

        /// <summary>
        /// Gets the average review score of the drink.
        /// </summary>
        public float AverageReviewScore { get; }
    }
}