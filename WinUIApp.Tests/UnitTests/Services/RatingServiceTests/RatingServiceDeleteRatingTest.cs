﻿using Moq;
using WinUIApp.WebAPI.Repositories;
using WinUIApp.WebAPI.Services;
using Xunit;

namespace WinUIApp.Tests.UnitTests.Services
{
    public class RatingServiceDeleteRatingTest
    {
        private readonly RatingService service;
        private readonly Mock<IRatingRepository> repository;

        public RatingServiceDeleteRatingTest()
        {
            repository = new Mock<IRatingRepository>();
            service = new RatingService(repository.Object);
        }

        [Fact]
        public void DeleteRatingById_WhenCalled_InvokesRepositoryDeleteOnce()
        {
            // Arrange
            int ratingId = 42;

            // Act
            service.DeleteRatingById(ratingId);

            // Assert
            repository.Verify(repo => repo.DeleteRating(ratingId), Times.Once);
        }
    }
}
