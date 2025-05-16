// <copyright file="ProxyDrinkService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.ProxyServices
{
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Json;
    using WinUIApp.Data.Requests.Drink;
    using WinUIApp.ProxyServices.Models;

    /// <summary>
    /// Proxy service for managing drink-related operations.
    /// </summary>
    public class ProxyDrinkService : IDrinkService
    {
        private const int DefaultPersonalDrinkCount = 1;
        private const float MinimumAlcoholPercentageConstant = 0f;
        private const float MaximumAlcoholPercentageConstant = 100f;
        private readonly HttpClient httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProxyDrinkService"/> class.
        /// </summary>
        public ProxyDrinkService()
        {
            httpClient = new HttpClient
            {
                BaseAddress = new Uri(GetApiUrl()),
            };
        }

        /// <summary>
        /// Gets a drink by its ID.
        /// </summary>
        /// <param name="drinkId"> Id of the drink. </param>
        /// <returns>The drink.</returns>
        /// <exception cref="Exception">Exceptions.</exception>
        public Drink? GetDrinkById(int drinkId)
        {
            try
            {
                var response = httpClient.GetAsync($"Drink/get-one?drinkId={drinkId}").Result;
                response.EnsureSuccessStatusCode();
                return response.Content.ReadFromJsonAsync<Drink>().Result;
            }
            catch (Exception exception)
            {
                throw new Exception($"Error happened while getting drink with ID {drinkId}:", exception);
            }
        }

        /// <summary>
        /// Gets a list of drinks based on various filters and ordering criteria.
        /// </summary>
        /// <param name="searchKeyword"> Search keyword.</param>
        /// <param name="drinkBrandNameFilter">Brand name filter.</param>
        /// <param name="drinkCategoryFilter">Category filter.</param>
        /// <param name="minimumAlcoholPercentage">Min. Alcohol percentage.</param>
        /// <param name="maximumAlcoholPercentage">Max. Alcohol percentage.</param>
        /// <param name="orderingCriteria">Order criteria.</param>
        /// <returns>List of drinks.</returns>
        /// <exception cref="Exception">Exceptions.</exception>
        public List<Drink> GetDrinks(string? searchKeyword, List<string>? drinkBrandNameFilter, List<string>? drinkCategoryFilter, float? minimumAlcoholPercentage, float? maximumAlcoholPercentage, Dictionary<string, bool>? orderingCriteria)
        {
            try
            {
                var request = new GetDrinksRequest
                {
                    searchKeyword = searchKeyword ?? string.Empty,
                    drinkBrandNameFilter = drinkBrandNameFilter ?? new List<string>(),
                    drinkCategoryFilter = drinkCategoryFilter ?? new List<string>(),
                    minimumAlcoholPercentage = minimumAlcoholPercentage ?? MinimumAlcoholPercentageConstant,
                    maximumAlcoholPercentage = maximumAlcoholPercentage ?? MaximumAlcoholPercentageConstant,
                    orderingCriteria = orderingCriteria,
                };

                var response = httpClient.PostAsJsonAsync("Drink/get-all", request).Result;
                response.EnsureSuccessStatusCode();
                return response.Content.ReadFromJsonAsync<List<Drink>>().Result;
            }
            catch (Exception exception)
            {
                throw new Exception("Error happened while getting drinks:", exception);
            }
        }

        /// <summary>
        /// Adds a drink to the database.
        /// </summary>
        /// <param name="inputtedDrinkName"> name. </param>
        /// <param name="inputtedDrinkPath"> imagepath. </param>
        /// <param name="inputtedDrinkCategories"> categories. </param>
        /// <param name="inputtedDrinkBrandName"> brand. </param>
        /// <param name="inputtedAlcoholPercentage"> alcohol. </param>
        /// <exception cref="Exception"> any issues. </exception>
        public void AddDrink(string inputtedDrinkName, string inputtedDrinkPath, List<Category> inputtedDrinkCategories, string inputtedDrinkBrandName, float inputtedAlcoholPercentage)
        {
            try
            {
                List<WinUiApp.Data.Data.Category> convertedCategories = new List<WinUiApp.Data.Data.Category>();
                foreach (var category in inputtedDrinkCategories)
                {
                    convertedCategories.Add(new WinUiApp.Data.Data.Category
                    {
                        CategoryId = category.CategoryId,
                        CategoryName = category.CategoryName,
                    });
                }

                var request = new AddDrinkRequest
                {
                    inputtedDrinkName = inputtedDrinkName,
                    inputtedDrinkPath = inputtedDrinkPath,
                    inputtedDrinkCategories = convertedCategories,
                    inputtedDrinkBrandName = inputtedDrinkBrandName,
                    inputtedAlcoholPercentage = inputtedAlcoholPercentage,
                };

                var response = httpClient.PostAsJsonAsync("Drink/add", request).Result;
                response.EnsureSuccessStatusCode();
            }
            catch (Exception exception)
            {
                throw new Exception("Error happened while adding a drink:", exception);
            }
        }

        /// <summary>
        /// Updates a drink in the database.
        /// </summary>
        /// <param name="drink"> drink. </param>
        /// <exception cref="Exception"> any issues. </exception>
        public void UpdateDrink(Drink drink)
        {
            WinUiApp.Data.Data.Drink convertedDrink = new WinUiApp.Data.Data.Drink
            {
                DrinkId = drink.DrinkId,
                DrinkName = drink.DrinkName,
                DrinkURL = drink.DrinkImageUrl,
                AlcoholContent = (decimal)drink.AlcoholContent,
                BrandId = drink.DrinkBrand.BrandId,
            };

            try
            {
                var request = new UpdateDrinkRequest { drink = convertedDrink };
                var response = httpClient.PutAsJsonAsync("Drink/update", request).Result;
                response.EnsureSuccessStatusCode();
            }
            catch (Exception exception)
            {
                throw new Exception("Error happened while updating a drink:", exception);
            }
        }

        /// <summary>
        /// Deletes a drink from the database.
        /// </summary>
        /// <param name="drinkId"> drink id. </param>
        /// <exception cref="Exception"> any issues. </exception>
        public void DeleteDrink(int drinkId)
        {
            try
            {
                var request = new DeleteDrinkRequest { drinkId = drinkId };
                var message = new HttpRequestMessage(HttpMethod.Delete, "Drink/delete")
                {
                    Content = JsonContent.Create(request),
                };
                var response = httpClient.SendAsync(message).Result;
                response.EnsureSuccessStatusCode();
            }
            catch (Exception exception)
            {
                throw new Exception("Error happened while deleting a drink:", exception);
            }
        }

        /// <summary>
        /// Retrieves drink categories.
        /// </summary>
        /// <returns> list of categories. </returns>
        /// <exception cref="Exception"> any issues. </exception>
        public List<Category> GetDrinkCategories()
        {
            try
            {
                var response = httpClient.GetAsync("Drink/get-drink-categories").Result;
                response.EnsureSuccessStatusCode();
                return response.Content.ReadFromJsonAsync<List<Category>>().Result;
            }
            catch (Exception exception)
            {
                throw new Exception("Error happened while getting drink categories:", exception);
            }
        }

        /// <summary>
        /// Retrieves a list of drink brands.
        /// </summary>
        /// <returns> list of brands. </returns>
        /// <exception cref="Exception"> any issues. </exception>
        public List<Brand> GetDrinkBrandNames()
        {
            try
            {
                var response = httpClient.GetAsync("Drink/get-drink-brands").Result;
                response.EnsureSuccessStatusCode();
                return response.Content.ReadFromJsonAsync<List<Brand>>().Result;
            }
            catch (Exception exception)
            {
                throw new Exception("Error happened while getting drink brands:", exception);
            }
        }

        /// <summary>
        /// Retrieves a random drink id from the database.
        /// </summary>
        /// <param name="userId"> user id. </param>
        /// <param name="maximumDrinkCount"> not sure. </param>
        /// <returns> personal list. </returns>
        /// <exception cref="Exception"> any issues. </exception>
        public List<Drink> GetUserPersonalDrinkList(int userId, int maximumDrinkCount = DefaultPersonalDrinkCount)
        {
            try
            {
                var request = new GetUserDrinkListRequest { userId = userId };
                var response = httpClient.PostAsJsonAsync("Drink/get-user-drink-list", request).Result;
                response.EnsureSuccessStatusCode();
                return response.Content.ReadFromJsonAsync<List<Drink>>().Result;
            }
            catch (Exception exception)
            {
                throw new Exception("Error getting personal drink list:", exception);
            }
        }

        /// <summary>
        /// Checks if a drink is already in the user's personal drink list.
        /// </summary>
        /// <param name="userId"> user id. </param>
        /// <param name="drinkId"> drink id. </param>
        /// <returns> true, if yes, false otherwise. </returns>
        /// <exception cref="Exception"> any issues. </exception>
        public bool IsDrinkInUserPersonalList(int userId, int drinkId)
        {
            try
            {
                var personalList = GetUserPersonalDrinkList(userId);
                return personalList.Any(drink => drink.DrinkId == drinkId);
            }
            catch (Exception exception)
            {
                throw new Exception("Error checking personal drink list:", exception);
            }
        }

        /// <summary>
        /// Adds a drink to the user's personal drink list.
        /// </summary>
        /// <param name="userId"> user id. </param>
        /// <param name="drinkId"> drink id. </param>
        /// <returns> true, if successfull, false otherwise. </returns>
        /// <exception cref="Exception"> any issues. </exception>
        public bool AddToUserPersonalDrinkList(int userId, int drinkId)
        {
            try
            {
                var request = new AddToUserPersonalDrinkListRequest
                {
                    userId = userId,
                    drinkId = drinkId
                };
                var response = httpClient.PostAsJsonAsync("Drink/add-to-user-drink-list", request).Result;
                response.EnsureSuccessStatusCode();
                return response.Content.ReadFromJsonAsync<bool>().Result;
            }
            catch (Exception exception)
            {
                throw new Exception("Error adding to personal list:", exception);
            }
        }

        /// <summary>
        /// Deletes a drink from the user's personal drink list.
        /// </summary>
        /// <param name="userId"> user id. </param>
        /// <param name="drinkId"> drink id. </param>
        /// <returns> true, if successfull, false otherwise. </returns>
        /// <exception cref="Exception"> any issues. </exception>
        public bool DeleteFromUserPersonalDrinkList(int userId, int drinkId)
        {
            try
            {
                var request = new DeleteFromUserPersonalDrinkListRequest
                {
                    userId = userId,
                    drinkId = drinkId,
                };
                var message = new HttpRequestMessage(HttpMethod.Delete, "Drink/delete-from-user-drink-list")
                {
                    Content = JsonContent.Create(request),
                };
                var response = httpClient.SendAsync(message).Result;
                response.EnsureSuccessStatusCode();
                return response.Content.ReadFromJsonAsync<bool>().Result;
            }
            catch (Exception exception)
            {
                throw new Exception("Error removing from personal list:", exception);
            }
        }

        /// <summary>
        /// Votes for the drink of the day.
        /// </summary>
        /// <param name="userId"> user id. </param>
        /// <param name="drinkId"> drink id. </param>
        /// <returns> the drink. </returns>
        /// <exception cref="Exception"> any issues. </exception>
        public Drink VoteDrinkOfTheDay(int userId, int drinkId)
        {
            try
            {
                var request = new VoteDrinkOfTheDayRequest
                {
                    userId = userId,
                    drinkId = drinkId,
                };
                var response = httpClient.PostAsJsonAsync("Drink/vote-drink-of-the-day", request).Result;
                response.EnsureSuccessStatusCode();
                return response.Content.ReadFromJsonAsync<Drink>().Result;
            }
            catch (Exception exception)
            {
                throw new Exception("Error voting for drink:", exception);
            }
        }

        /// <summary>
        /// Retrieves the drink of the day.
        /// </summary>
        /// <returns> the drink. </returns>
        /// <exception cref="Exception"> any issues. </exception>
        public Drink GetDrinkOfTheDay()
        {
            try
            {
                var response = httpClient.GetAsync("Drink/get-drink-of-the-day").Result;
                response.EnsureSuccessStatusCode();
                return response.Content.ReadFromJsonAsync<Drink>().Result ?? throw new Exception("Drink of the day not found");
            }
            catch (Exception exception)
            {
                throw new Exception("Error getting drink of the day:", exception);
            }
        }

        private string GetApiUrl()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            return configuration.GetValue<string>("ApiUrl");
        }
    }
}
