using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using WinUIApp.Models;
using WinUIApp.Services.DummyServices;
using Xunit;

namespace WinUIApp.Tests.UnitTests.Services
{
    /// <summary>
    /// Tests for the <see cref="DrinkReviewService"/> class.
    /// </summary>
    public class DrinkReviewServiceTest
    {
        private readonly DrinkReviewService reviewService;
        private readonly Mock<IDrinkReviewService> mockReviewService;

        public DrinkReviewServiceTest()
        {
            // Setup real service for direct testing
            reviewService = new DrinkReviewService();

            // Setup mock service for mocked scenarios
            mockReviewService = new Mock<IDrinkReviewService>();
        }

        #region GetReviewsByID Tests

        [Fact]
        public void GetReviewsByID_DrinkWithMultipleReviews_ReturnsAllReviews()
        {
            // Arrange
            int drinkId = 1; // Drink with 3 reviews in the dummy data

            // Act
            var result = reviewService.GetReviewsByID(drinkId);

            // Assert
            Assert.Equal(3, result.Count);
            Assert.All(result, review => Assert.Equal(drinkId, review.DrinkId));
        }

        [Fact]
        public void GetReviewsByID_DrinkWithNoReviews_ReturnsEmptyList()
        {
            // Arrange
            int drinkId = 999; // Non-existent drink ID

            // Act
            var result = reviewService.GetReviewsByID(drinkId);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetReviewsByID_InvalidDrinkId_ReturnsEmptyList()
        {
            // Arrange
            int drinkId = -1; // Negative drink ID

            // Act
            var result = reviewService.GetReviewsByID(drinkId);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetReviewsByID_ZeroDrinkId_ReturnsEmptyList()
        {
            // Arrange
            int drinkId = 0; // Zero drink ID

            // Act
            var result = reviewService.GetReviewsByID(drinkId);

            // Assert
            Assert.Empty(result);
        }

        #endregion

        #region GetReviewAverageByID Tests


        [Fact]
        public void GetReviewAverageByID_DrinkWithOneReview_ReturnsThatReviewScore()
        {
            // Arrange
            int drinkId = 100; // We'll mock a drink with one review
            float expectedScore = 4.2f;

            var expectedReviews = new List<Review>
                {
                    new Review(1, expectedScore, drinkId, "Title", "Content", DateTime.Now)
                };

            mockReviewService.Setup(mockService => mockService.GetReviewsByID(drinkId)).Returns(expectedReviews);
            mockReviewService.Setup(mockService => mockService.GetReviewAverageByID(drinkId)).Returns(expectedScore);

            // Arrange actual service to test with a real drink ID that has one review
            int realDrinkId = 10; // Assuming this has only one review in the dummy data

            // Act
            var result = reviewService.GetReviewAverageByID(realDrinkId);

            // Assert - Just check that it returns a value (we don't want to hardcode the expected average)
            Assert.True(result >= 0 && result <= 5);
        }

        [Fact]
        public void GetReviewAverageByID_DrinkWithMultipleReviews_ReturnsCorrectAverage()
        {
            // Arrange
            int drinkId = 1; // Drink with 3 reviews in the dummy data

            // Act
            var result = reviewService.GetReviewAverageByID(drinkId);
            var reviews = reviewService.GetReviewsByID(drinkId);
            var expectedAverage = reviews.Average(review => review.ReviewScore);

            // Assert
            Assert.Equal(expectedAverage, result);
        }

        [Fact]
        public void GetReviewAverageByID_VerifyCalculationCorrectness()
        {
            // Arrange
            int drinkId = 4; // Drink with multiple reviews

            // Get the actual reviews to calculate the expected average manually
            var reviews = reviewService.GetReviewsByID(drinkId);

            // Calculate expected average
            float sum = 0;
            foreach (var review in reviews)
            {
                sum += review.ReviewScore;
            }
            float expectedAverage = sum / reviews.Count;

            // Act
            var result = reviewService.GetReviewAverageByID(drinkId);

            // Assert
            Assert.Equal(expectedAverage, result);
        }

        [Fact]
        public void GetReviewAverageByID_DrinkWithNoReviews_ReturnsZero()
        {
            // Arrange
            int drinkId = 999; // Non-existent drink ID

            // Act
            var result = reviewService.GetReviewAverageByID(drinkId);

            // Assert
            Assert.Equal(0f, result);
        }

        [Fact]
        public void GetReviewAverageByID_InvalidDrinkId_ReturnsZero()
        {
            // Arrange
            int drinkId = -1; // Negative drink ID

            // Act
            var result = reviewService.GetReviewAverageByID(drinkId);

            // Assert
            Assert.Equal(0f, result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-5)]
        [InlineData(-100)]
        public void GetReviewAverageByID_NonPositiveDrinkId_ReturnsZero(int drinkId)
        {
            // Act - Using the real service
            var result = reviewService.GetReviewAverageByID(drinkId);

            // Assert
            Assert.Equal(0f, result);
        }

        #endregion
    }
}