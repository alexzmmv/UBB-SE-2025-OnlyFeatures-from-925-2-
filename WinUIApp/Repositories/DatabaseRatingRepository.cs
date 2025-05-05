﻿// <copyright file="DatabaseRatingRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using WinUIApp.Models;

    /// <summary>
    /// Repository class for handling operations related to ratings in the database.
    /// </summary>
    public class DatabaseRatingRepository : IRatingRepository
    {
        private readonly DatabaseConnection databaseConnection;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseRatingRepository"/> class.
        /// </summary>
        /// <param name="databaseConnection">The database connection.</param>
        public DatabaseRatingRepository(DatabaseConnection databaseConnection)
        {
            this.databaseConnection = databaseConnection;
        }

        /// <summary>
        /// Deletes a rating by its identifier.
        /// </summary>
        /// <param name="ratingId">The rating identifier.</param>
        public void DeleteRatingById(int ratingId)
        {
            using var connection = this.databaseConnection.CreateConnection();
            connection.Open();

            using var deleteRatingByIdCommand = DatabaseRatingRepositoryHelper.CreateDeleteRatingById(connection, ratingId);
            deleteRatingByIdCommand.ExecuteNonQuery();
        }

        /// <summary>
        /// Retrieves all ratings.
        /// </summary>
        /// <returns>A collection of ratings.</returns>
        public IEnumerable<Rating> GetAllRatings()
        {
            using var connection = this.databaseConnection.CreateConnection();
            connection.Open();

            using var getAllRatingsCommand = DatabaseRatingRepositoryHelper.CreateGetAllRatingsCommand(connection);
            using var reader = getAllRatingsCommand.ExecuteReader();

            return DatabaseRatingRepositoryHelper.ExhaustRatingReader(reader);
        }

        /// <summary>
        /// Retrieves a rating by its identifier.
        /// </summary>
        /// <param name="ratingId">The rating identifier.</param>
        /// <returns>The rating or null if it does not exist.</returns>
        public Rating? GetRatingById(int ratingId)
        {
            using var connection = this.databaseConnection.CreateConnection();
            connection.Open();

            using var getRatingByIdCommand = DatabaseRatingRepositoryHelper.CreateGetRatingByIdCommand(connection, ratingId);
            using var reader = getRatingByIdCommand.ExecuteReader();

            return DatabaseRatingRepositoryHelper.ExhaustRatingReader(reader).FirstOrDefault();
        }

        /// <summary>
        /// Retrieves ratings by product identifier.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <returns>A collection of ratings.</returns>
        public IEnumerable<Rating> GetRatingsByProductId(int productId)
        {
            using var connection = this.databaseConnection.CreateConnection();
            connection.Open();

            using var getRatingsByProductIdCommand = DatabaseRatingRepositoryHelper.CreateGetRatingsByProductIdCommand(connection, productId);
            using var reader = getRatingsByProductIdCommand.ExecuteReader();

            return DatabaseRatingRepositoryHelper.ExhaustRatingReader(reader);
        }

        /// <summary>
        /// Adds a new rating to the database.
        /// </summary>
        /// <param name="rating">The rating to add.</param>
        /// <returns>The added rating with its generated identifier.</returns>
        public Rating AddRating(Rating rating)
        {
            using var connection = this.databaseConnection.CreateConnection();
            connection.Open();

            using var addCommand = DatabaseRatingRepositoryHelper.CreateAddRatingCommand(connection, rating);
            rating.RatingId = (int)addCommand.ExecuteScalar();

            return rating;
        }

        /// <summary>
        /// Updates an existing rating in the database.
        /// </summary>
        /// <param name="rating">The rating to update.</param>
        /// <returns>The updated rating.</returns>
        public Rating UpdateRating(Rating rating)
        {
            using var connection = this.databaseConnection.CreateConnection();
            connection.Open();

            using var updateCommand = DatabaseRatingRepositoryHelper.CreateUpdateRatingCommand(connection, rating);
            updateCommand.ExecuteNonQuery();

            return rating;
        }

        /// <summary>
        /// Adds or updates a rating in the database.
        /// </summary>
        /// <param name="rating">The rating to add or update.</param>
        /// <returns>The added or updated rating.</returns>
        public Rating AddOrUpdateRating(Rating rating)
        {
            using var connection = this.databaseConnection.CreateConnection();
            connection.Open();

            using var checkIfExistsCommand = DatabaseRatingRepositoryHelper.CreateCheckIfRatingWithIdExistsCommand(connection, rating.RatingId);
            var existsRating = Convert.ToBoolean(checkIfExistsCommand.ExecuteScalar());

            if (existsRating)
            {
                return this.UpdateRating(rating);
            }

            return this.AddRating(rating);
        }
    }
}
