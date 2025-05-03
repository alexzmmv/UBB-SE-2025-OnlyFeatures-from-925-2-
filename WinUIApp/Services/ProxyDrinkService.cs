// <copyright file="ProxyDrinkService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Json;
    using WinUIApp.Data.Requests.Drink;
    using WinUIApp.Models;

    public class ProxyDrinkService : IDrinkService
    {
        private const int DefaultPersonalDrinkCount = 1;
        private readonly HttpClient httpClient;

        public ProxyDrinkService()
        {
            this.httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:5001/"),
            };
        }

        public Drink? GetDrinkById(int drinkId)
        {
            try
            {
                var response = httpClient.GetAsync($"Drink/get-one?drinkId={drinkId}").Result;
                response.EnsureSuccessStatusCode();
                return response.Content.ReadFromJsonAsync<Drink>().Result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error happened while getting drink with ID {drinkId}:", ex);
            }
        }

        public List<Drink> GetDrinks(string? searchKeyword, List<string>? drinkBrandNameFilter, List<string>? drinkCategoryFilter, float? minimumAlcoholPercentage, float? maximumAlcoholPercentage, Dictionary<string, bool>? orderingCriteria)
        {
            try
            {
                var request = new GetDrinksRequest
                {
                    searchKeyword = searchKeyword ?? string.Empty,
                    drinkBrandNameFilter = drinkBrandNameFilter ?? new List<string>(),
                    drinkCategoryFilter = drinkCategoryFilter ?? new List<string>(),
                    minimumAlcoholPercentage = minimumAlcoholPercentage ?? 0f,
                    maximumAlcoholPercentage = maximumAlcoholPercentage ?? 100f,
                    orderingCriteria = orderingCriteria
                };

                var response = httpClient.PostAsJsonAsync("Drink/get-all", request).Result;
                response.EnsureSuccessStatusCode();
                return response.Content.ReadFromJsonAsync<List<Drink>>().Result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error happened while getting drinks:", ex);
            }
        }

        public void AddDrink(string inputtedDrinkName, string inputtedDrinkPath, List<Category> inputtedDrinkCategories, string inputtedDrinkBrandName, float inputtedAlcoholPercentage)
        {
            try
            {
                var request = new AddDrinkRequest
                {
                    inputtedDrinkName = inputtedDrinkName,
                    inputtedDrinkPath = inputtedDrinkPath,
                    inputtedDrinkCategories = inputtedDrinkCategories,
                    inputtedDrinkBrandName = inputtedDrinkBrandName,
                    inputtedAlcoholPercentage = inputtedAlcoholPercentage,
                };

                var response = httpClient.PostAsJsonAsync("Drink/add", request).Result;
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                throw new Exception("Error happened while adding a drink:", ex);
            }
        }

        public void UpdateDrink(Drink drink)
        {
            try
            {
                var request = new UpdateDrinkRequest { drink = drink };
                var response = httpClient.PutAsJsonAsync("Drink/update", request).Result;
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                throw new Exception("Error happened while updating a drink:", ex);
            }
        }

        public void DeleteDrink(int drinkId)
        {
            try
            {
                var request = new DeleteDrinkRequest { drinkId = drinkId };
                var message = new HttpRequestMessage(HttpMethod.Delete, "Drink/delete")
                {
                    Content = JsonContent.Create(request)
                };
                var response = httpClient.SendAsync(message).Result;
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                throw new Exception("Error happened while deleting a drink:", ex);
            }
        }

        public List<Category> GetDrinkCategories()
        {
            try
            {
                var response = httpClient.GetAsync("Drink/get-drink-categories").Result;
                response.EnsureSuccessStatusCode();
                return response.Content.ReadFromJsonAsync<List<Category>>().Result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error happened while getting drink categories:", ex);
            }
        }

        public List<Brand> GetDrinkBrandNames()
        {
            try
            {
                var response = httpClient.GetAsync("Drink/get-drink-brands").Result;
                response.EnsureSuccessStatusCode();
                return response.Content.ReadFromJsonAsync<List<Brand>>().Result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error happened while getting drink brands:", ex);
            }
        }

        public List<Drink> GetUserPersonalDrinkList(int userId, int maximumDrinkCount = DefaultPersonalDrinkCount)
        {
            try
            {
                var request = new GetUserDrinkListRequest { userId = userId };
                var response = httpClient.PostAsJsonAsync("Drink/get-user-drink-list", request).Result;
                response.EnsureSuccessStatusCode();
                return response.Content.ReadFromJsonAsync<List<Drink>>().Result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting personal drink list:", ex);
            }
        }

        public bool IsDrinkInUserPersonalList(int userId, int drinkId)
        {
            try
            {
                var personalList = GetUserPersonalDrinkList(userId);
                return personalList.Any(d => d.DrinkId == drinkId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error checking personal drink list:", ex);
            }
        }

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
            catch (Exception ex)
            {
                throw new Exception("Error adding to personal list:", ex);
            }
        }

        public bool DeleteFromUserPersonalDrinkList(int userId, int drinkId)
        {
            try
            {
                var request = new DeleteFromUserPersonalDrinkListRequest
                {
                    userId = userId,
                    drinkId = drinkId
                };
                var message = new HttpRequestMessage(HttpMethod.Delete, "Drink/delete-from-user-drink-list")
                {
                    Content = JsonContent.Create(request)
                };
                var response = httpClient.SendAsync(message).Result;
                response.EnsureSuccessStatusCode();
                return response.Content.ReadFromJsonAsync<bool>().Result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error removing from personal list:", ex);
            }
        }

        public Drink VoteDrinkOfTheDay(int userId, int drinkId)
        {
            try
            {
                var request = new VoteDrinkOfTheDayRequest
                {
                    userId = userId,
                    drinkId = drinkId
                };
                var response = httpClient.PostAsJsonAsync("Drink/vote-drink-of-the-day", request).Result;
                response.EnsureSuccessStatusCode();
                return response.Content.ReadFromJsonAsync<Drink>().Result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error voting for drink:", ex);
            }
        }

        public Drink GetDrinkOfTheDay()
        {
            try
            {
                var response = httpClient.GetAsync("Drink/get-drink-of-the-day").Result;
                response.EnsureSuccessStatusCode();
                return response.Content.ReadFromJsonAsync<Drink>().Result ?? throw new Exception("Drink of the day not found");
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting drink of the day:", ex);
            }
        }
    }
}