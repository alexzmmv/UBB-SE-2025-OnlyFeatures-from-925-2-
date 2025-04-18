// <copyright file="DrinkServiceTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.Tests.UnitTests.Services
{
    using System;
    using System.Collections.Generic;
    using Xunit;
    using Moq;
    using WinUIApp.Models;
    using WinUIApp.Repositories;
    using WinUIApp.Services;

    /// <summary>
    /// Unit tests for the <see cref="DrinkService"/> class.
    /// </summary>
    public class DrinkServiceTests
    {
        private readonly Mock<IDrinkRepository> mockRepository;
        private readonly DrinkService drinkService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DrinkServiceTests"/> class.
        /// </summary>
        public DrinkServiceTests()
        {
            this.mockRepository = new Mock<IDrinkRepository>();
            this.drinkService = new DrinkService(this.mockRepository.Object);
        }

        /// <summary>
        /// Test for GetDrinkById - should return the drink when it exists.
        /// </summary>
        [Fact]
        public void GetDrinkById_ValidId_ReturnsDrink()
        {
            // Arrange
            int drinkId = 1;
            var expectedDrink = new Drink(drinkId, "Test Drink", "test.jpg", new List<Category>(), new Brand(1, "Test Brand"), 5.0f);
            this.mockRepository.Setup(r => r.GetDrinkById(drinkId)).Returns(expectedDrink);

            // Act
            var result = this.drinkService.GetDrinkById(drinkId);

            // Assert
            Assert.Equal(expectedDrink, result);
            this.mockRepository.Verify(r => r.GetDrinkById(drinkId), Times.Once);
        }

        /// <summary>
        /// Test for GetDrinkById - should wrap and rethrow exception.
        /// </summary>
        [Fact]
        public void GetDrinkById_RepositoryThrowsException_WrapException()
        {
            // Arrange
            int drinkId = 1;
            var exception = new Exception("Repository error");
            this.mockRepository.Setup(r => r.GetDrinkById(drinkId)).Throws(exception);

            // Act & Assert
            var thrownException = Assert.Throws<Exception>(() => this.drinkService.GetDrinkById(drinkId));
            Assert.Contains($"Error happened while getting drink with ID {drinkId}", thrownException.Message);
            Assert.Equal(exception, thrownException.InnerException);
        }

        /// <summary>
        /// Test for GetDrinks - should return all drinks with no filters.
        /// </summary>
        [Fact]
        public void GetDrinks_NoFilters_ReturnsAllDrinks()
        {
            // Arrange
            var expectedDrinks = new List<Drink>
            {
                new Drink(1, "Test Drink 1", "test1.jpg", new List<Category>(), new Brand(1, "Brand 1"), 5.0f),
                new Drink(2, "Test Drink 2", "test2.jpg", new List<Category>(), new Brand(2, "Brand 2"), 8.0f)
            };
            this.mockRepository.Setup(repository => repository.GetDrinks()).Returns(expectedDrinks);

            // Act
            var result = this.drinkService.GetDrinks(searchKeyword: null, drinkBrandNameFilter: null, 
                drinkCategoryFilter: null, minimumAlcoholPercentage: null, 
                maximumAlcoholPercentage: null, orderingCriteria: null);

            // Assert
            Assert.Equal(expectedDrinks.Count, result.Count);
            Assert.Equal(expectedDrinks, result);
            this.mockRepository.Verify(r => r.GetDrinks(), Times.Once);
        }

        /// <summary>
        /// Test for GetDrinks - should filter by minimum alcohol percentage.
        /// </summary>
        [Fact]
        public void GetDrinks_WithMinimumAlcoholFilter_ReturnsFilteredDrinks()
        {
            // Arrange
            float minAlcohol = 6.0f;
            var allDrinks = new List<Drink>
            {
                new Drink(1, "Test Drink 1", "test1.jpg", new List<Category>(), new Brand(1, "Brand 1"), 5.0f),
                new Drink(2, "Test Drink 2", "test2.jpg", new List<Category>(), new Brand(2, "Brand 2"), 8.0f)
            };
            this.mockRepository.Setup(repository => repository.GetDrinks()).Returns(allDrinks);

            // Act
            var result = this.drinkService.GetDrinks(searchKeyword: null, drinkBrandNameFilter: null, 
                drinkCategoryFilter: null, minimumAlcoholPercentage: minAlcohol, 
                maximumAlcoholPercentage: null, orderingCriteria: null);

            // Assert
            Assert.Single(result);
            Assert.Equal(8.0f, result[0].AlcoholContent);
            this.mockRepository.Verify(r => r.GetDrinks(), Times.Once);
        }

        /// <summary>
        /// Test for GetDrinks - should filter by maximum alcohol percentage.
        /// </summary>
        [Fact]
        public void GetDrinks_WithMaximumAlcoholFilter_ReturnsFilteredDrinks()
        {
            // Arrange
            float maxAlcohol = 6.0f;
            var allDrinks = new List<Drink>
            {
                new Drink(1, "Test Drink 1", "test1.jpg", new List<Category>(), new Brand(1, "Brand 1"), 5.0f),
                new Drink(2, "Test Drink 2", "test2.jpg", new List<Category>(), new Brand(2, "Brand 2"), 8.0f)
            };
            this.mockRepository.Setup(repository => repository.GetDrinks()).Returns(allDrinks);

            // Act
            var result = this.drinkService.GetDrinks(searchKeyword: null, drinkBrandNameFilter: null, 
                drinkCategoryFilter: null, minimumAlcoholPercentage: null, 
                maximumAlcoholPercentage: maxAlcohol, orderingCriteria: null);

            // Assert
            Assert.Single(result);
            Assert.Equal(5.0f, result[0].AlcoholContent);
            this.mockRepository.Verify(r => r.GetDrinks(), Times.Once);
        }

        /// <summary>
        /// Test for GetDrinks - should order by alcohol percentage ascending.
        /// </summary>
        [Fact]
        public void GetDrinks_WithOrderingByAlcoholAscending_ReturnsOrderedDrinks()
        {
            // Arrange
            var allDrinks = new List<Drink>
            {
                new Drink(2, "Test Drink 2", "test2.jpg", new List<Category>(), new Brand(2, "Brand 2"), 8.0f),
                new Drink(1, "Test Drink 1", "test1.jpg", new List<Category>(), new Brand(1, "Brand 1"), 5.0f)
            };
            this.mockRepository.Setup(repository => repository.GetDrinks()).Returns(allDrinks);

            var orderingCriteria = new Dictionary<string, bool> { { "AlcoholContent", true } };

            // Act
            var result = this.drinkService.GetDrinks(searchKeyword: null, drinkBrandNameFilter: null, 
                drinkCategoryFilter: null, minimumAlcoholPercentage: null, 
                maximumAlcoholPercentage: null, orderingCriteria: orderingCriteria);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(5.0f, result[0].AlcoholContent);
            Assert.Equal(8.0f, result[1].AlcoholContent);
            this.mockRepository.Verify(r => r.GetDrinks(), Times.Once);
        }

        /// <summary>
        /// Test for GetDrinks - should order by alcohol percentage descending.
        /// </summary>
        [Fact]
        public void GetDrinks_WithOrderingByAlcoholDescending_ReturnsOrderedDrinks()
        {
            // Arrange
            var allDrinks = new List<Drink>
            {
                new Drink(1, "Test Drink 1", "test1.jpg", new List<Category>(), new Brand(1, "Brand 1"), 5.0f),
                new Drink(2, "Test Drink 2", "test2.jpg", new List<Category>(), new Brand(2, "Brand 2"), 8.0f)
            };
            this.mockRepository.Setup(repository => repository.GetDrinks()).Returns(allDrinks);

            var orderingCriteria = new Dictionary<string, bool> { { "AlcoholContent", false } };

            // Act
            var result = this.drinkService.GetDrinks(searchKeyword: null, drinkBrandNameFilter: null, 
                drinkCategoryFilter: null, minimumAlcoholPercentage: null, 
                maximumAlcoholPercentage: null, orderingCriteria: orderingCriteria);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(8.0f, result[0].AlcoholContent);
            Assert.Equal(5.0f, result[1].AlcoholContent);
            this.mockRepository.Verify(r => r.GetDrinks(), Times.Once);
        }

        /// <summary>
        /// Test for GetDrinks - should wrap and rethrow exception.
        /// </summary>
        [Fact]
        public void GetDrinks_RepositoryThrowsException_WrapException()
        {
            // Arrange
            var exception = new Exception("Repository error");
            this.mockRepository.Setup(repository => repository.GetDrinks()).Throws(exception);

            // Act & Assert
            var thrownException = Assert.Throws<Exception>(() => this.drinkService.GetDrinks(
                searchKeyword: null, drinkBrandNameFilter: null, drinkCategoryFilter: null,
                minimumAlcoholPercentage: null, maximumAlcoholPercentage: null, orderingCriteria: null));
                
            Assert.Contains("Error happened while getting drinks", thrownException.Message);
            Assert.Equal(exception, thrownException.InnerException);
        }

        /// <summary>
        /// Test for AddDrink - should call repository.
        /// </summary>
        [Fact]
        public void AddDrink_ValidParameters_CallsRepository()
        {
            // Arrange
            string drinkName = "New Drink";
            string drinkUrl = "new.jpg";
            var categories = new List<Category> { new Category(1, "Test Category") };
            string brandName = "Test Brand";
            float alcoholContent = 5.0f;

            // Act
            this.drinkService.AddDrink(drinkName, drinkUrl, categories, brandName, alcoholContent);

            // Assert
            this.mockRepository.Verify(r => r.AddDrink(drinkName, drinkUrl, categories, brandName, alcoholContent), Times.Once);
        }

        /// <summary>
        /// Test for AddDrink - should wrap and rethrow exception.
        /// </summary>
        [Fact]
        public void AddDrink_RepositoryThrowsException_WrapException()
        {
            // Arrange
            string drinkName = "New Drink";
            string drinkUrl = "new.jpg";
            var categories = new List<Category> { new Category(1, "Test Category") };
            string brandName = "Test Brand";
            float alcoholContent = 5.0f;
            var exception = new Exception("Repository error");
            
            this.mockRepository.Setup(r => r.AddDrink(drinkName, drinkUrl, categories, brandName, alcoholContent))
                .Throws(exception);

            // Act & Assert
            var thrownException = Assert.Throws<Exception>(() => 
                this.drinkService.AddDrink(drinkName, drinkUrl, categories, brandName, alcoholContent));
            Assert.Contains("Error happened while adding a drink", thrownException.Message);
            Assert.Equal(exception, thrownException.InnerException);
        }

        /// <summary>
        /// Test for UpdateDrink - should call repository.
        /// </summary>
        [Fact]
        public void UpdateDrink_ValidDrink_CallsRepository()
        {
            // Arrange
            var drink = new Drink(1, "Updated Drink", "updated.jpg", new List<Category>(), new Brand(1, "Brand"), 7.5f);

            // Act
            this.drinkService.UpdateDrink(drink);

            // Assert
            this.mockRepository.Verify(r => r.UpdateDrink(drink), Times.Once);
        }

        /// <summary>
        /// Test for UpdateDrink - should wrap and rethrow exception.
        /// </summary>
        [Fact]
        public void UpdateDrink_RepositoryThrowsException_WrapException()
        {
            // Arrange
            var drink = new Drink(1, "Updated Drink", "updated.jpg", new List<Category>(), new Brand(1, "Brand"), 7.5f);
            var exception = new Exception("Repository error");
            this.mockRepository.Setup(r => r.UpdateDrink(drink)).Throws(exception);

            // Act & Assert
            var thrownException = Assert.Throws<Exception>(() => this.drinkService.UpdateDrink(drink));
            Assert.Contains("Error happened while updating a drink", thrownException.Message);
            Assert.Equal(exception, thrownException.InnerException);
        }

        /// <summary>
        /// Test for DeleteDrink - should call repository.
        /// </summary>
        [Fact]
        public void DeleteDrink_ValidId_CallsRepository()
        {
            // Arrange
            int drinkId = 1;

            // Act
            this.drinkService.DeleteDrink(drinkId);

            // Assert
            this.mockRepository.Verify(r => r.DeleteDrink(drinkId), Times.Once);
        }

        /// <summary>
        /// Test for DeleteDrink - should wrap and rethrow exception.
        /// </summary>
        [Fact]
        public void DeleteDrink_RepositoryThrowsException_WrapException()
        {
            // Arrange
            int drinkId = 1;
            var exception = new Exception("Repository error");
            this.mockRepository.Setup(r => r.DeleteDrink(drinkId)).Throws(exception);

            // Act & Assert
            var thrownException = Assert.Throws<Exception>(() => this.drinkService.DeleteDrink(drinkId));
            Assert.Contains("Error happened while deleting a drink", thrownException.Message);
            Assert.Equal(exception, thrownException.InnerException);
        }

        /// <summary>
        /// Test for GetDrinkCategories - should return categories.
        /// </summary>
        [Fact]
        public void GetDrinkCategories_NoErrors_ReturnsCategories()
        {
            // Arrange
            var expectedCategories = new List<Category>
            {
                new Category(1, "Category 1"),
                new Category(2, "Category 2")
            };
            this.mockRepository.Setup(r => r.GetDrinkCategories()).Returns(expectedCategories);

            // Act
            var result = this.drinkService.GetDrinkCategories();

            // Assert
            Assert.Equal(expectedCategories, result);
            this.mockRepository.Verify(r => r.GetDrinkCategories(), Times.Once);
        }

        /// <summary>
        /// Test for GetDrinks - should filter by search keyword.
        /// </summary>
        [Fact]
        public void GetDrinks_WithSearchKeyword_ReturnsFilteredDrinks()
        {
            // Arrange
            var allDrinks = new List<Drink>
            {
                new Drink(1, "Lime Shot", "lime.jpg", new List<Category>(), new Brand(1, "Brand A"), 5.0f),
                new Drink(2, "Berry Juice", "berry.jpg", new List<Category>(), new Brand(2, "Brand B"), 4.0f)
            };
            this.mockRepository.Setup(repository => repository.GetDrinks()).Returns(allDrinks);

            // Act
            var result = this.drinkService.GetDrinks("Lime", null, null, null, null, null);

            // Assert
            Assert.Single(result);
        }

        /// <summary>
        /// Test for GetDrinks - should filter by brand name.
        /// </summary>
        [Fact]
        public void GetDrinks_WithBrandFilter_ReturnsFilteredDrinks()
        {
            // Arrange
            var allDrinks = new List<Drink>
            {
                new Drink(1, "Drink A", "a.jpg", new List<Category>(), new Brand(1, "TargetBrand"), 5.0f),
                new Drink(2, "Drink B", "b.jpg", new List<Category>(), new Brand(2, "OtherBrand"), 5.0f)
            };
            this.mockRepository.Setup(repository => repository.GetDrinks()).Returns(allDrinks);

            // Act
            var result = this.drinkService.GetDrinks(null, new List<string> { "TargetBrand" }, null, null, null, null);

            // Assert
            Assert.Single(result);
        }

        /// <summary>
        /// Test for GetDrinks - should filter by category name.
        /// </summary>
        [Fact]
        public void GetDrinks_WithCategoryFilter_ReturnsFilteredDrinks()
        {
            // Arrange
            var allDrinks = new List<Drink>
            {
                new Drink(1, "Drink A", "a.jpg", new List<Category> { new Category(1, "Shot") }, new Brand(1, "Brand"), 5.0f),
                new Drink(2, "Drink B", "b.jpg", new List<Category> { new Category(2, "Cocktail") }, new Brand(1, "Brand"), 5.0f)
            };
            this.mockRepository.Setup(repository => repository .GetDrinks()).Returns(allDrinks);

            // Act
            var result = this.drinkService.GetDrinks(null, null, new List<string> { "Shot" }, null, null, null);

            // Assert
            Assert.Single(result);
        }

        /// <summary>
        /// Test for GetDrinks - should order by name ascending.
        /// </summary>
        [Fact]
        public void GetDrinks_WithOrderingByNameAscending_ReturnsOrderedDrinks()
        {
            // Arrange
            var allDrinks = new List<Drink>
            {
                new Drink(1, "Zeta", "z.jpg", new List<Category>(), new Brand(1, "Brand A"), 5.0f),
                new Drink(2, "Alpha", "a.jpg", new List<Category>(), new Brand(2, "Brand B"), 5.0f)
            };
            this.mockRepository.Setup(repository => repository.GetDrinks()).Returns(allDrinks);

            var orderingCriteria = new Dictionary<string, bool> { { "DrinkName", true } };

            // Act
            var result = this.drinkService.GetDrinks(null, null, null, null, null, orderingCriteria);

            // Assert
            Assert.Equal("Alpha", result[0].DrinkName);
        }

        /// <summary>
        /// Test for GetDrinks - should order by name descending.
        /// </summary>
        [Fact]
        public void GetDrinks_WithOrderingByNameDescending_ReturnsOrderedDrinks()
        {
            // Arrange
            var allDrinks = new List<Drink>
            {
                new Drink(1, "Alpha", "a.jpg", new List<Category>(), new Brand(1, "Brand A"), 5.0f),
                new Drink(2, "Zeta", "z.jpg", new List<Category>(), new Brand(2, "Brand B"), 5.0f)
            };
            this.mockRepository.Setup(repository => repository.GetDrinks()).Returns(allDrinks);

            var orderingCriteria = new Dictionary<string, bool> { { "DrinkName", false } };

            // Act
            var result = this.drinkService.GetDrinks(null, null, null, null, null, orderingCriteria);

            // Assert
            Assert.Equal("Zeta", result[0].DrinkName);
        }

        /// <summary>
        /// Test for GetDrinks - should ignore ordering if multiple keys provided.
        /// </summary>
        [Fact]
        public void GetDrinks_WithMultipleOrderingKeys_IgnoresOrdering()
        {
            // Arrange
            var allDrinks = new List<Drink>
            {
                new Drink(1, "B Drink", "b.jpg", new List<Category>(), new Brand(1, "Brand"), 5.0f),
                new Drink(2, "A Drink", "a.jpg", new List<Category>(), new Brand(2, "Brand"), 8.0f)
            };
            this.mockRepository.Setup(repository => repository.GetDrinks()).Returns(allDrinks);

            var orderingCriteria = new Dictionary<string, bool>
            {
                { "DrinkName", true },
                { "AlcoholContent", false }
            };

            // Act
            var result = this.drinkService.GetDrinks(null, null, null, null, null, orderingCriteria);

            // Assert
            Assert.Equal(2, result.Count);
        }

        /// <summary>
        /// Test for GetDrinks - should ignore empty search keyword.
        /// </summary>
        [Fact]
        public void GetDrinks_WithEmptySearchKeyword_IgnoresSearch()
        {
            // Arrange
            var allDrinks = new List<Drink>
            {
                new Drink(1, "Drink One", "1.jpg", new List<Category>(), new Brand(1, "Brand"), 5.0f),
                new Drink(2, "Drink Two", "2.jpg", new List<Category>(), new Brand(1, "Brand"), 5.0f)
            };
            this.mockRepository.Setup(repository => repository.GetDrinks()).Returns(allDrinks);

            // Act
            var result = this.drinkService.GetDrinks("", null, null, null, null, null);

            // Assert
            Assert.Equal(2, result.Count);
        }
        /// <summary>
        /// Test for GetDrinks - should filter by search, brand, and category.
        /// </summary>
        [Fact]
        public void GetDrinks_WithSearchBrandAndCategoryFilters_ReturnsFilteredDrink()
        {
            // Arrange
            var allDrinks = new List<Drink>
            {
                new Drink(1, "Lemon Shot", "img1.jpg", new List<Category> { new Category(1, "Shot") }, new Brand(1, "CoolBrand"), 5.0f),
                new Drink(2, "Berry Vodka", "img2.jpg", new List<Category> { new Category(2, "Vodka") }, new Brand(2, "OtherBrand"), 5.0f)
            };
            this.mockRepository.Setup(repository => repository.GetDrinks()).Returns(allDrinks);

            // Act
            var result = this.drinkService.GetDrinks("Lemon", new List<string> { "CoolBrand" }, new List<string> { "Shot" }, null, null, null);

            // Assert
            Assert.Single(result);
        }

        /// <summary>
        /// Test for GetDrinks - should filter by brand, category, and alcohol range.
        /// </summary>
        [Fact]
        public void GetDrinks_WithBrandCategoryAndAlcoholRangeFilters_ReturnsFilteredDrink()
        {
            // Arrange
            var allDrinks = new List<Drink>
            {
                new Drink(1, "Zippy Ale", "img1.jpg", new List<Category> { new Category(1, "Ale") }, new Brand(1, "HopBrand"), 6.5f),
                new Drink(2, "Dark Lager", "img2.jpg", new List<Category> { new Category(2, "Lager") }, new Brand(2, "BrewBrand"), 9.0f)
            };
            this.mockRepository.Setup(repository => repository.GetDrinks()).Returns(allDrinks);

            // Act
            var result = this.drinkService.GetDrinks(null, new List<string> { "HopBrand" }, new List<string> { "Ale" }, 6.0f, 7.0f, null);

            // Assert
            Assert.Single(result);
        }

        /// <summary>
        /// Test for GetDrinks - should apply all filters and sort by alcohol descending.
        /// </summary>
        [Fact]
        public void GetDrinks_WithAllFiltersAndAlcoholOrderingDesc_ReturnsSortedDrink()
        {
            // Arrange
            var allDrinks = new List<Drink>
            {
                new Drink(1, "Citrus Pop", "img1.jpg", new List<Category> { new Category(1, "Cocktail") }, new Brand(1, "ZingBrand"), 4.5f),
                new Drink(2, "Citrus Bomb", "img2.jpg", new List<Category> { new Category(1, "Cocktail") }, new Brand(1, "ZingBrand"), 6.5f)
            };
            this.mockRepository.Setup(repository => repository.GetDrinks()).Returns(allDrinks);

            var ordering = new Dictionary<string, bool> { { "AlcoholContent", false } };

            // Act
            var result = this.drinkService.GetDrinks("Citrus", new List<string> { "ZingBrand" }, new List<string> { "Cocktail" }, 4.0f, 7.0f, ordering);

            // Assert
            Assert.Equal(6.5f, result[0].AlcoholContent);
        }

        /// <summary>
        /// Test for GetDrinks - should filter by search keyword and alcohol range.
        /// </summary>
        [Fact]
        public void GetDrinks_WithSearchAndAlcoholRange_ReturnsFilteredDrink()
        {
            // Arrange
            var allDrinks = new List<Drink>
            {
                new Drink(1, "Rum Delight", "rum1.jpg", new List<Category>(), new Brand(1, "Brand X"), 6.0f),
                new Drink(2, "Rum Strong", "rum2.jpg", new List<Category>(), new Brand(2, "Brand Y"), 9.0f)
            };
            this.mockRepository.Setup(repository => repository.GetDrinks()).Returns(allDrinks);

            // Act
            var result = this.drinkService.GetDrinks("Rum", null, null, 5.0f, 7.0f, null);

            // Assert
            Assert.Single(result);
        }

        /// <summary>
        /// Test for GetDrinks - should order search results by name ascending.
        /// </summary>
        [Fact]
        public void GetDrinks_WithSearchAndOrderingByNameAscending_ReturnsOrderedDrinks()
        {
            // Arrange
            var allDrinks = new List<Drink>
            {
                new Drink(1, "Mint Storm", "m1.jpg", new List<Category>(), new Brand(1, "Fresh"), 5.0f),
                new Drink(2, "Mint Breeze", "m2.jpg", new List<Category>(), new Brand(1, "Fresh"), 5.0f)
            };
            this.mockRepository.Setup(repository => repository.GetDrinks()).Returns(allDrinks);

            var ordering = new Dictionary<string, bool> { { "DrinkName", true } };

            // Act
            var result = this.drinkService.GetDrinks("Mint", null, null, null, null, ordering);

            // Assert
            Assert.Equal("Mint Breeze", result[0].DrinkName);
        }

        /// <summary>
        /// Test for GetDrinks - should order brand-filtered drinks by name descending.
        /// </summary>
        [Fact]
        public void GetDrinks_WithBrandFilterAndOrderingByNameDescending_ReturnsOrderedDrinks()
        {
            // Arrange
            var allDrinks = new List<Drink>
            {
                new Drink(1, "Berry", "b.jpg", new List<Category>(), new Brand(1, "ChillBrand"), 5.0f),
                new Drink(2, "Apple", "a.jpg", new List<Category>(), new Brand(1, "ChillBrand"), 5.0f)
            };
            this.mockRepository.Setup(repository => repository.GetDrinks()).Returns(allDrinks);

            var ordering = new Dictionary<string, bool> { { "DrinkName", false } };

            // Act
            var result = this.drinkService.GetDrinks(null, new List<string> { "ChillBrand" }, null, null, null, ordering);

            // Assert
            Assert.Equal("Berry", result[0].DrinkName);
        }

        /// <summary>
        /// Test for GetDrinks - should filter by category and minimum alcohol percentage.
        /// </summary>
        [Fact]
        public void GetDrinks_WithCategoryAndMinAlcoholFilter_ReturnsFilteredDrink()
        {
            // Arrange
            var allDrinks = new List<Drink>
            {
                new Drink(1, "Gin Fizz", "g1.jpg", new List<Category> { new Category(1, "Cocktail") }, new Brand(1, "Brand A"), 7.0f),
                new Drink(2, "Gin Light", "g2.jpg", new List<Category> { new Category(1, "Cocktail") }, new Brand(1, "Brand A"), 4.0f)
            };
            this.mockRepository.Setup(repository => repository.GetDrinks()).Returns(allDrinks);

            // Act
            var result = this.drinkService.GetDrinks(null, null, new List<string> { "Cocktail" }, 5.0f, null, null);

            // Assert
            Assert.Single(result);
        }

        /// <summary>
        /// Test for GetDrinks - should filter by search, brand, category and order by name ascending.
        /// </summary>
        [Fact]
        public void GetDrinks_WithSearchBrandCategoryAndNameOrderingAsc_ReturnsOrderedDrink()
        {
            // Arrange
            var allDrinks = new List<Drink>
            {
                new Drink(1, "Berry Boost", "bb.jpg", new List<Category> { new Category(1, "Juice") }, new Brand(1, "FruitBrand"), 4.5f),
                new Drink(2, "Berry Blast", "bl.jpg", new List<Category> { new Category(1, "Juice") }, new Brand(1, "FruitBrand"), 4.5f)
            };
            this.mockRepository.Setup(repository => repository.GetDrinks()).Returns(allDrinks);

            var ordering = new Dictionary<string, bool> { { "DrinkName", true } };

            // Act
            var result = this.drinkService.GetDrinks("Berry", new List<string> { "FruitBrand" }, new List<string> { "Juice" }, null, null, ordering);

            // Assert
            Assert.Equal("Berry Blast", result[0].DrinkName);
        }

        /// Test for VoteDrinkOfTheDay - should return drink on new vote.
        /// </summary>
        [Fact]
        public void VoteDrinkOfTheDay_NewVote_ReturnsDrink()
        {
            // Arrange
            int userId = 1;
            int drinkId = 42;
            var expectedDrink = new Drink(drinkId, "Lager", "lager.png", new List<Category>(), new Brand(1, "Brand"), 5.0f);

            this.mockRepository.Setup(r => r.VoteDrinkOfTheDay(userId, drinkId));
            this.mockRepository.Setup(r => r.GetDrinkById(drinkId)).Returns(expectedDrink);

            // Act
            var result = this.drinkService.VoteDrinkOfTheDay(userId, drinkId);

            // Assert
            Assert.Equal(expectedDrink, result);
            this.mockRepository.Verify(r => r.VoteDrinkOfTheDay(userId, drinkId), Times.Once);
            this.mockRepository.Verify(r => r.GetDrinkById(drinkId), Times.Once);
        }

        /// <summary>
        /// Test for VoteDrinkOfTheDay - should return updated drink if user modifies vote.
        /// </summary>
        [Fact]
        public void VoteDrinkOfTheDay_ModifiedVote_ReturnsDrink()
        {
            // Arrange
            int userId = 2;
            int drinkId = 7;
            var expectedDrink = new Drink(drinkId, "Stout", "stout.jpg", new List<Category>(), new Brand(2, "Brew"), 6.5f);

            this.mockRepository.Setup(r => r.VoteDrinkOfTheDay(userId, drinkId));
            this.mockRepository.Setup(r => r.GetDrinkById(drinkId)).Returns(expectedDrink);

            // Act
            var result = this.drinkService.VoteDrinkOfTheDay(userId, drinkId);

            // Assert
            Assert.Equal(expectedDrink, result);
            this.mockRepository.Verify(r => r.VoteDrinkOfTheDay(userId, drinkId), Times.Once);
            this.mockRepository.Verify(r => r.GetDrinkById(drinkId), Times.Once);
        }

        /// <summary>
        /// Test for VoteDrinkOfTheDay - should wrap and rethrow exception if repository fails.
        /// </summary>
        [Fact]
        public void VoteDrinkOfTheDay_WhenRepositoryThrows_ThrowsWrappedException()
        {
            // Arrange
            int userId = 1;
            int drinkId = 99;
            var exception = new Exception("DB error");

            this.mockRepository.Setup(r => r.VoteDrinkOfTheDay(userId, drinkId)).Throws(exception);

            // Act & Assert
            var thrown = Assert.Throws<Exception>(() => this.drinkService.VoteDrinkOfTheDay(userId, drinkId));
            Assert.Contains("Error voting drink", thrown.Message);
            Assert.Equal(exception, thrown.InnerException);
        }

        /// <summary>
        /// Test for GetDrinkOfTheDay - should return the drink of the day successfully.
        /// </summary>
        [Fact]
        public void GetDrinkOfTheDay_RepositoryReturnsDrink_ReturnsDrink()
        {
            // Arrange
            var expectedDrink = new Drink(1, "Daily Special", "daily.jpg", new List<Category>(), new Brand(1, "Brand X"), 6.5f);
            this.mockRepository.Setup(r => r.GetDrinkOfTheDay()).Returns(expectedDrink);

            // Act
            var result = this.drinkService.GetDrinkOfTheDay();

            // Assert
            Assert.Equal(expectedDrink, result);
            this.mockRepository.Verify(r => r.GetDrinkOfTheDay(), Times.Once);
        }

        /// <summary>
        /// Test for GetDrinkOfTheDay - should wrap and rethrow exception when repository fails.
        /// </summary>
        [Fact]
        public void GetDrinkOfTheDay_RepositoryThrowsException_WrapsException()
        {
            // Arrange
            var exception = new Exception("Repository error");
            this.mockRepository.Setup(r => r.GetDrinkOfTheDay()).Throws(exception);

            // Act & Assert
            var thrown = Assert.Throws<Exception>(() => this.drinkService.GetDrinkOfTheDay());
            Assert.Contains("Error getting drink of the day", thrown.Message);
            Assert.Equal(exception, thrown.InnerException);
        }

    }
}