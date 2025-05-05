﻿// <copyright file="DatabaseReviewRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using WinUIApp.Models;

    /// <summary>
    /// Provides methods for managing reviews in the database.
    /// </summary>
    public class DatabaseReviewRepository : IReviewRepository
    {
        private readonly DatabaseConnection databaseConnection;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseReviewRepository"/> class.
        /// </summary>
        /// <param name="databaseConnection">The database connection to use.</param>
        public DatabaseReviewRepository(DatabaseConnection databaseConnection)
        {
            this.databaseConnection = databaseConnection ?? throw new ArgumentNullException(nameof(databaseConnection));
        }

        /// <summary>
        /// Deletes a review by its unique identifier.
        /// </summary>
        /// <param name="reviewId">The unique identifier of the review to delete.</param>
        public void DeleteReviewById(int reviewId)
        {
            using var connection = this.databaseConnection.CreateConnection();
            connection.Open();

            using var deleteReviewByIdCommand = DatabaseReviewRepositoryHelper.CreateDeleteReviewById(connection, reviewId);
            deleteReviewByIdCommand.ExecuteNonQuery();
        }

        /// <summary>
        /// Retrieves all reviews from the database.
        /// </summary>
        /// <returns>A collection of all reviews.</returns>
        public IEnumerable<Review> GetAllReviews()
        {
            using var connection = this.databaseConnection.CreateConnection();
            connection.Open();

            using var getAllReviewsCommand = DatabaseReviewRepositoryHelper.CreateGetAllReviewsCommand(connection);
            using var reader = getAllReviewsCommand.ExecuteReader();

            return DatabaseReviewRepositoryHelper.ExhaustReviewReader(reader);
        }

        /// <summary>
        /// Retrieves a review by its unique identifier.
        /// </summary>
        /// <param name="reviewId">The unique identifier of the review to retrieve.</param>
        /// <returns>The review with the specified identifier.</returns>
        public Review? GetReviewById(int reviewId)
        {
            using var connection = this.databaseConnection.CreateConnection();
            connection.Open();

            using var getReviewByIdCommand = DatabaseReviewRepositoryHelper.CreateGetReviewByIdCommand(connection, reviewId);
            using var reader = getReviewByIdCommand.ExecuteReader();

            return DatabaseReviewRepositoryHelper.ExhaustReviewReader(reader).FirstOrDefault();
        }

        /// <summary>
        /// Retrieves all reviews associated with a specific rating identifier.
        /// </summary>
        /// <param name="ratingId">The rating identifier to filter reviews by.</param>
        /// <returns>A collection of reviews associated with the specified rating identifier.</returns>
        public IEnumerable<Review> GetReviewsByRatingId(int ratingId)
        {
            using var connection = this.databaseConnection.CreateConnection();
            connection.Open();

            using var getReviewsByRatingIdCommand = DatabaseReviewRepositoryHelper.CreateGetReviewsByRatingIdCommand(connection, ratingId);
            using var reader = getReviewsByRatingIdCommand.ExecuteReader();

            return DatabaseReviewRepositoryHelper.ExhaustReviewReader(reader);
        }

        /// <summary>
        /// Checks whether a review with the specified identifier exists in the database.
        /// </summary>
        /// <param name="reviewId">The unique identifier of the review to check.</param>
        /// <returns><c>true</c> if the review exists; otherwise, <c>false</c>.</returns>
        public bool CheckIfReviewWithIdExists(int reviewId)
        {
            using var connection = this.databaseConnection.CreateConnection();
            connection.Open();

            using var checkIfReviewWithIdExistsCommand = DatabaseReviewRepositoryHelper.CreateCheckIfReviewWithIdExistsCommand(connection, reviewId);
            var doesReviewExist = Convert.ToBoolean(checkIfReviewWithIdExistsCommand.ExecuteScalar());

            return doesReviewExist;
        }

        /// <summary>
        /// Adds a new review to the database.
        /// </summary>
        /// <param name="review">The review to add.</param>
        /// <returns>The unique identifier of the newly added review.</returns>
        public int AddReview(Review review)
        {
            using var connection = this.databaseConnection.CreateConnection();
            connection.Open();

            using var addCommand = DatabaseReviewRepositoryHelper.CreateAddReviewCommand(connection, review);
            return (int)addCommand.ExecuteScalar();
        }

        /// <summary>
        /// Updates an existing review in the database.
        /// </summary>
        /// <param name="review">The review to update.</param>
        /// <returns>The updated review.</returns>
        public Review UpdateReview(Review review)
        {
            using var connection = this.databaseConnection.CreateConnection();
            connection.Open();

            using var updateCommand = DatabaseReviewRepositoryHelper.CreateUpdateReviewCommand(connection, review);
            updateCommand.ExecuteNonQuery();

            return review;
        }

        /// <summary>
        /// Adds a new review or updates an existing one in the database.
        /// </summary>
        /// <param name="review">The review to add or update.</param>
        /// <returns>The added or updated review.</returns>
        public Review AddOrUpdateReview(Review review)
        {
            var reviewExists = this.CheckIfReviewWithIdExists(review.ReviewId);
            if (reviewExists)
            {
                this.UpdateReview(review);
            }
            else
            {
                review.ReviewId = this.AddReview(review);
            }

            return review;
        }
    }
}
