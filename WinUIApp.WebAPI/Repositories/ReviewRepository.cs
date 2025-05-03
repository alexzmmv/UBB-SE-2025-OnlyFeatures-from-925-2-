// <copyright file="ReviewRepository.cs" company="PlaceholderCompany">
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
    /// Repository for managing review-related operations.
    /// </summary>
    internal class ReviewRepository : IReviewRepository
    {
        private readonly AppDbContext dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReviewRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public ReviewRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <inheritdoc/>
        public Review? GetReviewById(int reviewId)
        {
            return this.dbContext.Reviews
                .FirstOrDefault(r => r.ReviewId == reviewId);
        }

        /// <inheritdoc/>
        public List<Review> GetAllReviews()
        {
            return this.dbContext.Reviews.ToList();
        }

        /// <inheritdoc/>
        public List<Review> GetReviewsByRatingId(int ratingId)
        {
            return this.dbContext.Reviews
                .Where(r => r.RatingId == ratingId)
                .ToList();
        }

        /// <inheritdoc/>
        public int AddReview(Review review)
        {
            if (string.IsNullOrWhiteSpace(review.Content))
            {
                throw new ArgumentException("Review content cannot be null or empty.", nameof(review.Content));
            }

            review.CreationDate ??= DateTime.UtcNow;
            review.IsActive ??= true;

            this.dbContext.Reviews.Add(review);
            this.dbContext.SaveChanges();

            return review.ReviewId;
        }

        /// <inheritdoc/>
        public void UpdateReview(Review review)
        {
            if (string.IsNullOrWhiteSpace(review.Content))
            {
                throw new ArgumentException("Review content cannot be null or empty.", nameof(review.Content));
            }

            var existingReview = this.dbContext.Reviews
                .FirstOrDefault(r => r.ReviewId == review.ReviewId);

            if (existingReview == null)
            {
                throw new Exception("No review found with the provided Id.");
            }

            existingReview.RatingId = review.RatingId;
            existingReview.UserId = review.UserId;
            existingReview.Content = review.Content;
            existingReview.CreationDate = review.CreationDate ?? existingReview.CreationDate;
            existingReview.IsActive = review.IsActive ?? existingReview.IsActive;

            this.dbContext.SaveChanges();
        }

        /// <inheritdoc/>
        public void DeleteReview(int reviewId)
        {
            var review = this.dbContext.Reviews
                .FirstOrDefault(r => r.ReviewId == reviewId);

            if (review == null)
            {
                throw new Exception("No review found with the provided Id.");
            }

            this.dbContext.Reviews.Remove(review);
            this.dbContext.SaveChanges();
        }

        /// <inheritdoc/>
        public bool CheckIfReviewExists(int reviewId)
        {
            return this.dbContext.Reviews.Any(r => r.ReviewId == reviewId);
        }
    }
}
