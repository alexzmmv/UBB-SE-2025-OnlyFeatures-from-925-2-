// <copyright file="ReviewExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.WebAPI.Extensions
{
    using System;
    using WinUiApp.Data.Data;
    using WinUIApp.WebAPI.Constants;

    /// <summary>
    /// Extension methods for the <see cref="Review"/> class.
    /// </summary>
    public static class ReviewExtensions
    {
        /// <summary>
        /// Checks if a review is valid.
        /// </summary>
        /// <param name="review">The review to validate.</param>
        /// <returns>True if the review is valid; otherwise, false.</returns>
        public static bool IsValid(this Review review)
        {
            return review != null 
                && !string.IsNullOrWhiteSpace(review.Content) 
                && review.Content.Length <= ReviewDomainConstants.MaxContentLength;
        }

        /// <summary>
        /// Activates the review, setting its creation date to the current time.
        /// </summary>
        /// <param name="review">The review to activate.</param>
        public static void Activate(this Review review)
        {
            if (review == null)
            {
                throw new ArgumentNullException(nameof(review));
            }

            review.IsActive = true;
            review.CreationDate = DateTime.Now;
        }
    }
} 