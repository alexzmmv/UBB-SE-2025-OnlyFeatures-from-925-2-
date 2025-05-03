﻿//// <copyright file="DrinkService.cs" company="PlaceholderCompany">
//// Copyright (c) PlaceholderCompany. All rights reserved.
//// </copyright>

//namespace WinUIApp.Services
//{
//    using System;
//    using System.Collections.Generic;
//    using System.Linq;
//    using WinUiApp.Data.Data;
//    using WinUIApp.Repositories;

//    /// <summary>
//    /// Drink service for managing drink-related operations.
//    /// </summary>
//    public class DrinkService : IDrinkService
//    {
//        private const int DefaultPersonalDrinkCount = 1;
//        private IDrinkRepository drinkRepository;

//        /// <summary>
//        /// Initializes a new instance of the <see cref="DrinkService"/> class.
//        /// </summary>
//        public DrinkService()
//        {
//            this.drinkRepository = new DrinkRepository();
//        }

//        /// <summary>
//        /// Initializes a new instance of the <see cref="DrinkService"/> class with a specified drink model.
//        /// </summary>
//        /// <param name="drinkRepository"> Drink Repository. </param>
//        public DrinkService(IDrinkRepository drinkRepository)
//        {
//            this.drinkRepository = drinkRepository;
//        }

//        /// <summary>
//        /// Gets the drink by ID.
//        /// </summary>
//        /// <param name="drinkId"> Drink id. </param>
//        /// <returns> The drink. </returns>
//        /// <exception cref="Exception"> Any issues. </exception>
//        public Drink? GetDrinkById(int drinkId)
//        {
//            try
//            {
//                return this.drinkRepository.GetDrinkById(drinkId);
//            }
//            catch (Exception drinkRetrievalException)
//            {
//                throw new Exception($"Error happened while getting drink with ID {drinkId}:", drinkRetrievalException);
//            }
//        }

//        /// <summary>
//        /// Gets drinks based on various filters and ordering criteria.
//        /// </summary>
//        /// <param name="searchKeyword"> search term. </param>
//        /// <param name="drinkBrandNameFilter"> brand filter. </param>
//        /// <param name="drinkCategoryFilter"> category filter. </param>
//        /// <param name="minimumAlcoholPercentage"> minimum alcohol content. </param>
//        /// <param name="maximumAlcoholPercentage"> maximum alcohol content. </param>
//        /// <param name="orderingCriteria"> order criteria. </param>
//        /// <returns> List of drinks. </returns>
//        /// <exception cref="Exception"> Any issues. </exception>
//        public List<Drink> GetDrinks(string? searchKeyword, List<string>? drinkBrandNameFilter, List<string>? drinkCategoryFilter, float? minimumAlcoholPercentage, float? maximumAlcoholPercentage, Dictionary<string, bool>? orderingCriteria)
//        {
//            try
//            {
//                List<Drink> allDrinks = this.drinkRepository.GetDrinks();
//                List<Drink> filteredDrinks = allDrinks;

//                if (minimumAlcoholPercentage != null)
//                {
//                    filteredDrinks = filteredDrinks.FindAll(drink => drink.AlcoholContent >= minimumAlcoholPercentage);
//                }

//                if (maximumAlcoholPercentage != null)
//                {
//                    filteredDrinks = filteredDrinks.FindAll(drink => drink.AlcoholContent <= maximumAlcoholPercentage);
//                }

//                if (searchKeyword != null && searchKeyword != string.Empty)
//                {
//                    filteredDrinks = filteredDrinks.FindAll(drink => drink.DrinkName.ToLower().Contains(searchKeyword.ToLower()));
//                }

//                if (drinkBrandNameFilter != null && drinkBrandNameFilter.Count > 0)
//                {
//                    filteredDrinks = filteredDrinks.FindAll(drink => drinkBrandNameFilter.Contains(drink.DrinkBrand.BrandName));
//                }

//                if (drinkCategoryFilter != null && drinkCategoryFilter.Count > 0)
//                {
//                    filteredDrinks = filteredDrinks.FindAll(drink =>
//                        drinkCategoryFilter.TrueForAll(categoryFilter =>
//                            drink.CategoryList.Any(category => category.CategoryName == categoryFilter)));
//                }

//                if (orderingCriteria != null && orderingCriteria.Count == 1)
//                {
//                    string orderingKey = orderingCriteria.Keys.First();
//                    bool isAscending = orderingCriteria[orderingKey];

//                    if (orderingKey == "DrinkName")
//                    {
//                        if (isAscending)
//                        {
//                            filteredDrinks = filteredDrinks.OrderBy(drink => drink.DrinkName).ToList();
//                        }
//                        else
//                        {
//                            filteredDrinks = filteredDrinks.OrderByDescending(drink => drink.DrinkName).ToList();
//                        }
//                    }
//                    else if (orderingKey == "AlcoholContent")
//                    {
//                        if (isAscending)
//                        {
//                            filteredDrinks = filteredDrinks.OrderBy(drink => drink.AlcoholContent).ToList();
//                        }
//                        else
//                        {
//                            filteredDrinks = filteredDrinks.OrderByDescending(drink => drink.AlcoholContent).ToList();
//                        }
//                    }
//                }

//                return filteredDrinks;
//            }
//            catch (Exception drinksRetrievalException)
//            {
//                throw new Exception("Error happened while getting drinks:", drinksRetrievalException);
//            }
//        }

//        /// <summary>
//        /// Adds a drink to the database.
//        /// </summary>
//        /// <param name="inputtedDrinkName"> Name. </param>
//        /// <param name="inputtedDrinkPath"> ImagePath. </param>
//        /// <param name="inputtedDrinkCategories"> Categories. </param>
//        /// <param name="inputtedDrinkBrandName"> Brand. </param>
//        /// <param name="inputtedAlcoholPercentage"> Alcohol. </param>
//        /// <exception cref="Exception"> Any issues. </exception>
//        public void AddDrink(string inputtedDrinkName, string inputtedDrinkPath, List<Category> inputtedDrinkCategories, string inputtedDrinkBrandName, float inputtedAlcoholPercentage)
//        {
//            try
//            {
//                this.drinkRepository.AddDrink(inputtedDrinkName, inputtedDrinkPath, inputtedDrinkCategories, inputtedDrinkBrandName, inputtedAlcoholPercentage);
//            }
//            catch (Exception addingDrinkException)
//            {
//                throw new Exception("Error happened while adding a drink:", addingDrinkException);
//            }
//        }

//        /// <summary>
//        /// Updates a drink in the database.
//        /// </summary>
//        /// <param name="drink"> Drink. </param>
//        /// <exception cref="Exception"> Any issues. </exception>
//        public void UpdateDrink(Drink drink)
//        {
//            try
//            {
//                this.drinkRepository.UpdateDrink(drink);
//            }
//            catch (Exception updateDrinkException)
//            {
//                throw new Exception("Error happened while updating a drink:", updateDrinkException);
//            }
//        }

//        /// <summary>
//        /// Deletes a drink from the database.
//        /// </summary>
//        /// <param name="drinkId"> Drink id. </param>
//        /// <exception cref="Exception"> Any issues. </exception>
//        public void DeleteDrink(int drinkId)
//        {
//            try
//            {
//                this.drinkRepository.DeleteDrink(drinkId);
//            }
//            catch (Exception deleteDrinkException)
//            {
//                throw new Exception("Error happened while deleting a drink:", deleteDrinkException);
//            }
//        }

//        /// <summary>
//        /// Retrieves a random drink ID from the database.
//        /// </summary>
//        /// <returns> List of categories. </returns>
//        /// <exception cref="Exception"> Any issues. </exception>
//        public List<Category> GetDrinkCategories()
//        {
//            try
//            {
//                return this.drinkRepository.GetDrinkCategories();
//            }
//            catch (Exception drinkCategoriesRetrievalException)
//            {
//                throw new Exception("Error happened while getting drink categories:", drinkCategoriesRetrievalException);
//            }
//        }

//        /// <summary>
//        /// Retrieves a list of drink brands.
//        /// </summary>
//        /// <returns> List of brands. </returns>
//        /// <exception cref="Exception"> Any issues. </exception>
//        public List<Brand> GetDrinkBrandNames()
//        {
//            try
//            {
//                return this.drinkRepository.GetDrinkBrands();
//            }
//            catch (Exception drinkBrandNamesRetrievalException)
//            {
//                throw new Exception("Error happened while getting drink brands:", drinkBrandNamesRetrievalException);
//            }
//        }

//        /// <summary>
//        /// Retrieves a random drink ID from the database.
//        /// </summary>
//        /// <param name="userId"> User id. </param>
//        /// <param name="maximumDrinkCount"> Not sure. </param>
//        /// <returns> Personal list. </returns>
//        /// <exception cref="Exception"> Any issues. </exception>
//        public List<Drink> GetUserPersonalDrinkList(int userId, int maximumDrinkCount = DefaultPersonalDrinkCount)
//        {
//            try
//            {
//                return this.drinkRepository.GetPersonalDrinkList(userId);
//            }
//            catch (Exception personalDrinkListRetrievalException)
//            {
//                throw new Exception("Error getting personal drink list:", personalDrinkListRetrievalException);
//            }
//        }

//        /// <summary>
//        /// Checks if a drink is already in the user's personal drink list.
//        /// </summary>
//        /// <param name="userId"> User id. </param>
//        /// <param name="drinkId"> Drink id. </param>
//        /// <returns> true, if yes, false otherwise. </returns>
//        /// <exception cref="Exception"> Any issues. </exception>
//        public bool IsDrinkInUserPersonalList(int userId, int drinkId)
//        {
//            try
//            {
//                return this.drinkRepository.IsDrinkInPersonalList(userId, drinkId);
//            }
//            catch (Exception checkingUserPersonalListException)
//            {
//                throw new Exception("Error checking if the drink is in the user's personal list.", checkingUserPersonalListException);
//            }
//        }

//        /// <summary>
//        /// Adds a drink to the user's personal drink list.
//        /// </summary>
//        /// <param name="userId"> User id. </param>
//        /// <param name="drinkId"> Drink id. </param>
//        /// <returns> True, if successfull, false otherwise. </returns>
//        /// <exception cref="Exception"> Any issues. </exception>
//        public bool AddToUserPersonalDrinkList(int userId, int drinkId)
//        {
//            try
//            {
//                return this.drinkRepository.AddToPersonalDrinkList(userId, drinkId);
//            }
//            catch (Exception addDrinkToUserPersonalListException)
//            {
//                throw new Exception("Error adding drink to personal list:", addDrinkToUserPersonalListException);
//            }
//        }

//        /// <summary>
//        /// Deletes a drink from the user's personal drink list.
//        /// </summary>
//        /// <param name="userId"> User id. </param>
//        /// <param name="drinkId"> Drink id. </param>
//        /// <returns> True, if successfull, false otherwise. </returns>
//        /// <exception cref="Exception"> Any issues. </exception>
//        public bool DeleteFromUserPersonalDrinkList(int userId, int drinkId)
//        {
//            try
//            {
//                return this.drinkRepository.DeleteFromPersonalDrinkList(userId, drinkId);
//            }
//            catch (Exception deleteFromUserPersonalDrinkListException)
//            {
//                throw new Exception("Error deleting drink from personal list:", deleteFromUserPersonalDrinkListException);
//            }
//        }

//        /// <summary>
//        /// Votes for the drink of the day.
//        /// </summary>
//        /// <param name="userId"> User id. </param>
//        /// <param name="drinkId"> Drink id. </param>
//        /// <returns> The drink. </returns>
//        /// <exception cref="Exception"> Any issues. </exception>
//        public Drink VoteDrinkOfTheDay(int userId, int drinkId)
//        {
//            try
//            {
//                this.drinkRepository.VoteDrinkOfTheDay(userId, drinkId);
//                return this.drinkRepository.GetDrinkById(drinkId) ?? throw new Exception("Drink not found after voting.");
//            }
//            catch (Exception voteDrinkOfTheDayException)
//            {
//                throw new Exception("Error voting drink:", voteDrinkOfTheDayException);
//            }
//        }

//        /// <summary>
//        /// Retrieves the drink of the day.
//        /// </summary>
//        /// <returns> The drink. </returns>
//        /// <exception cref="Exception"> Any issues. </exception>
//        public Drink GetDrinkOfTheDay()
//        {
//            try
//            {
//                return this.drinkRepository.GetDrinkOfTheDay();
//            }
//            catch (Exception e)
//            {
//                throw new Exception("Error getting drink of the day:" + e.Message, e);
//            }
//        }
//    }
//}