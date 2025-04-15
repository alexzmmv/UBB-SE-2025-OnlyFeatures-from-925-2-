// <copyright file="DrinkRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Data.SqlClient;
    using WinUIApp.Database;
    using WinUIApp.Models;

    /// <summary>
    /// Repository for managing drink-related operations.
    /// </summary>
    internal class DrinkRepository : IDrinkRepository
    {
        private const int ZeroResults = 0;
        private const int FirstResult = 0;

        private readonly DatabaseManager dataBaseManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="DrinkRepository"/> class.
        /// </summary>
        public DrinkRepository()
        {
            this.dataBaseManager = DatabaseManager.Instance;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DrinkRepository"/> class.
        /// </summary>
        /// <param name="dataBaseService"> The database service. </param>
        public DrinkRepository(DatabaseManager dataBaseService)
        {
            this.dataBaseManager = dataBaseService;
        }

        /// <summary>
        /// Retrieves a list of all drinks.
        /// </summary>
        /// <returns> List of drinks. </returns>
        public List<Drink> GetDrinks()
        {
            try
            {
                string getDrinksQuery = "SELECT * FROM Drink;";
                var selectResult = this.dataBaseManager.ExecuteSelectQuery(getDrinksQuery);
                List<Drink> drinks = new List<Drink>();
                foreach (var drink in selectResult)
                {
                    int id = Convert.ToInt32(drink["DrinkId"]);
                    string drinkName = drink["DrinkName"].ToString();
                    string drinkImageUrl = drink["DrinkURL"].ToString();
                    float alcoholContent = Convert.ToSingle(drink["AlcoholContent"]);
                    Brand brand = this.GetBrandById(id);
                    List<Category> categories = this.GetDrinkCategoriesById(id);
                    drinks.Add(new Drink(id, drinkName, drinkImageUrl, categories, brand, alcoholContent));
                }

                return drinks;
            }
            catch (Exception exception)
            {
                throw new Exception("Database error occurred while retrieving drinks." + exception.Message, exception);
            }
        }

        /// <summary>
        /// Retrieves a drink by its unique identifier.
        /// </summary>
        /// <param name="drinkId"> Drink id. </param>
        /// <returns> The drink. </returns>
        public Drink? GetDrinkById(int drinkId)
        {
            try
            {
                string getDrinkQuery = "SELECT * FROM Drink WHERE DrinkId = @drinkId;";
                var parameters = new List<SqlParameter>
                {
                    new ("@drinkId", SqlDbType.Int) { Value = drinkId },
                };
                var selectResult = this.dataBaseManager.ExecuteSelectQuery(getDrinkQuery, parameters);
                if (selectResult.Count == ZeroResults)
                {
                    throw new Exception("No drink found with the provided ID.");
                }

                int id = Convert.ToInt32(selectResult[FirstResult]["DrinkId"]);
                string drinkName = selectResult[FirstResult]["DrinkName"].ToString();
                string drinkImageUrl = selectResult[FirstResult]["DrinkURL"].ToString();
                float alcoholContent = Convert.ToSingle(selectResult[FirstResult]["AlcoholContent"]);
                Brand brand = this.GetBrandById(id);
                List<Category> categories = this.GetDrinkCategoriesById(id);

                return new Drink(id, drinkName, drinkImageUrl, categories, brand, alcoholContent);
            }
            catch (Exception exception)
            {
                throw new Exception("Database error occurred while retrieving drink by ID." + exception.Message, exception);
            }
        }

        /// <summary>
        /// Adds a new drink to the database.
        /// </summary>
        /// <param name="drinkName"> Drink name. </param>
        /// <param name="drinkUrl"> Drink Url. </param>
        /// <param name="categories"> List of categories. </param>
        /// <param name="brandName"> Brand name. </param>
        /// <param name="alcoholContent"> Alcohol content. </param>
        public void AddDrink(string drinkName, string drinkUrl, List<Category> categories, string brandName, float alcoholContent)
        {
            try
            {
                if (this.IsBrandInDatabase(brandName) == false)
                {
                    this.AddBrand(brandName);
                }

                string getBrandIdQuery = "SELECT BrandId FROM Brand WHERE BrandName = @brandName;";
                var parameters = new List<SqlParameter>
                {
                    new ("@brandName", SqlDbType.VarChar) { Value = brandName },
                };
                var selectResult = this.dataBaseManager.ExecuteSelectQuery(getBrandIdQuery, parameters);
                if (selectResult.Count == ZeroResults)
                {
                    throw new Exception("No brand found with the provided name.");
                }

                int brandId = Convert.ToInt32(selectResult[FirstResult]["BrandId"]);

                string addDrinkQuery = "INSERT INTO Drink (DrinkName, DrinkURL, AlcoholContent, BrandId) VALUES (@drinkName, @drinkUrl, @alcoholContent, @brandId);";
                var drinkParameters = new List<SqlParameter>
                {
                    new ("@drinkName", SqlDbType.VarChar) { Value = drinkName },
                    new ("@drinkUrl", SqlDbType.VarChar) { Value = drinkUrl },
                    new ("@alcoholContent", SqlDbType.Float) { Value = alcoholContent },
                    new ("@brandId", SqlDbType.Int) { Value = brandId },
                };
                this.dataBaseManager.ExecuteDataModificationQuery(addDrinkQuery, drinkParameters);

                string getDrinkIdQuery = "SELECT DrinkId FROM Drink WHERE DrinkName = @drinkName;";
                var drinkIdParameters = new List<SqlParameter>
                {
                    new ("@drinkName", SqlDbType.NVarChar) { Value = drinkName },
                };
                var drinkIdResult = this.dataBaseManager.ExecuteSelectQuery(getDrinkIdQuery, drinkIdParameters);
                if (drinkIdResult.Count == ZeroResults)
                {
                    throw new Exception("Failed to create drink.");
                }

                int drinkId = Convert.ToInt32(drinkIdResult[FirstResult]["DrinkId"]);

                string addDrinkCategoryQuery = "INSERT INTO DrinkCategory (DrinkId, CategoryId) VALUES (@drinkId, @categoryId);";
                foreach (var category in categories)
                {
                    var categoryParameters = new List<SqlParameter>
                    {
                        new ("@drinkId", SqlDbType.Int) { Value = drinkId },
                        new ("@categoryId", SqlDbType.Int) { Value = category.CategoryId },
                    };
                    this.dataBaseManager.ExecuteDataModificationQuery(addDrinkCategoryQuery, categoryParameters);
                }
            }
            catch (Exception exception)
            {
                throw new Exception("Database error occurred while adding a drink." + exception.Message, exception);
            }
        }

        /// <summary>
        /// Updates the details of an existing drink in the database.
        /// </summary>
        /// <param name="drink"> The drink with updated info. </param>
        public void UpdateDrink(Drink drink)
        {
            try
            {
                if (this.IsBrandInDatabase(drink.DrinkBrand.BrandName) == false)
                {
                    this.AddBrand(drink.DrinkBrand.BrandName);
                }

                string getBrandIdQuery = "SELECT BrandId FROM Brand WHERE BrandName = @brandName;";
                var parameters = new List<SqlParameter>
                {
                    new ("@brandName", SqlDbType.VarChar) { Value = drink.DrinkBrand.BrandName },
                };
                var selectResult = this.dataBaseManager.ExecuteSelectQuery(getBrandIdQuery, parameters);
                if (selectResult.Count == ZeroResults)
                {
                    throw new Exception("Error creating new brand on drink update.");
                }

                int brandId = Convert.ToInt32(selectResult[FirstResult]["BrandId"]);

                string updateDrinkQuery = "UPDATE Drink SET DrinkName = @drinkName, DrinkURL = @drinkUrl, AlcoholContent = @alcoholContent, BrandId = @brandId WHERE DrinkId = @drinkId;";
                var drinkParameters = new List<SqlParameter>
                {
                    new ("@drinkId", SqlDbType.Int) { Value = drink.DrinkId },
                    new ("@drinkName", SqlDbType.VarChar) { Value = drink.DrinkName },
                    new ("@drinkUrl", SqlDbType.VarChar) { Value = drink.DrinkImageUrl },
                    new ("@alcoholContent", SqlDbType.Float) { Value = drink.AlcoholContent },
                    new ("@brandId", SqlDbType.Int) { Value = brandId },
                };
                this.dataBaseManager.ExecuteDataModificationQuery(updateDrinkQuery, drinkParameters);

                string deleteDrinkCategoryQuery = "DELETE FROM DrinkCategory WHERE DrinkId = @drinkId;";
                var deleteParameters = new List<SqlParameter>
                {
                    new ("@drinkId", SqlDbType.Int) { Value = drink.DrinkId },
                };
                this.dataBaseManager.ExecuteDataModificationQuery(deleteDrinkCategoryQuery, deleteParameters);

                string addDrinkCategoryQuery = "INSERT INTO DrinkCategory (DrinkId, CategoryId) VALUES (@drinkId, @categoryId);";
                foreach (var category in drink.CategoryList)
                {
                    var categoryParameters = new List<SqlParameter>
                    {
                        new ("@drinkId", SqlDbType.Int) { Value = drink.DrinkId },
                        new ("@categoryId", SqlDbType.Int) { Value = category.CategoryId },
                    };
                    this.dataBaseManager.ExecuteDataModificationQuery(addDrinkCategoryQuery, categoryParameters);
                }
            }
            catch (Exception exception)
            {
                throw new Exception("Database error occurred while updating drink." + exception.Message, exception);
            }
        }

        /// <summary>
        /// Deletes a drink from the database.
        /// </summary>
        /// <param name="drinkId"> Drink id. </param>
        public void DeleteDrink(int drinkId)
        {
            try
            {
                string deleteDrinkCategoryQuery = "DELETE FROM DrinkCategory WHERE DrinkId = @drinkId;";
                var deleteParameters = new List<SqlParameter>
                {
                    new ("@drinkId", SqlDbType.Int) { Value = drinkId },
                };
                this.dataBaseManager.ExecuteDataModificationQuery(deleteDrinkCategoryQuery, deleteParameters);

                string deleteDrinkQuery = "DELETE FROM Drink WHERE DrinkId = @drinkId;";
                var drinkParameters = new List<SqlParameter>
                {
                    new ("@drinkId", SqlDbType.Int) { Value = drinkId },
                };
                this.dataBaseManager.ExecuteDataModificationQuery(deleteDrinkQuery, drinkParameters);
            }
            catch (Exception exception)
            {
                throw new Exception("Database error occurred while deleting drink." + exception.Message, exception);
            }
        }

        /// <summary>
        /// Retrieves the drink of the day.
        /// </summary>
        /// <returns> Drink of the day. </returns>
        public Drink GetDrinkOfTheDay()
        {
            try
            {
                string checkDrinkOfTheDayQuery = "SELECT COUNT(*) as Count FROM DrinkOfTheDay WHERE CONVERT(date, DrinkTime) = CONVERT(date, @date);";
                var parameters = new List<SqlParameter>
                {
                    new ("@date", SqlDbType.DateTime) { Value = DateTime.UtcNow },
                };
                var selectResult = this.dataBaseManager.ExecuteSelectQuery(checkDrinkOfTheDayQuery, parameters);
                int count = Convert.ToInt32(selectResult[FirstResult]["Count"]);
                if (count == ZeroResults)
                {
                    this.ResetDrinkOfTheDay();
                }

                string getDrinkOfTheDayQuery = "SELECT DrinkId FROM DrinkOfTheDay;";
                selectResult = this.dataBaseManager.ExecuteSelectQuery(getDrinkOfTheDayQuery);
                if (selectResult.Count == ZeroResults)
                {
                    throw new Exception("An issue occured in the DrinkOfTheDay table, check if there is anything in it.");
                }

                int drinkId = Convert.ToInt32(selectResult[FirstResult]["DrinkId"]);
                Drink? drink = this.GetDrinkById(drinkId);
                if (drink == null)
                {
                    throw new Exception("The drink id has been successfully retrieved, however the drink itself either doesn't exist or there was an issue retrieving it.");
                }

                return drink;
            }
            catch (Exception exception)
            {
                throw new Exception("Database error occurred while retrieving drink of the day." + exception.Message, exception);
            }
        }

        /// <summary>
        /// Resets the Drink of the Day to the new top-voted drink for today.
        /// </summary>
        public void ResetDrinkOfTheDay()
        {
            try
            {
                string clearDrinkOfTheDayQuery = "DELETE FROM DrinkOfTheDay;";
                this.dataBaseManager.ExecuteDataModificationQuery(clearDrinkOfTheDayQuery);

                int drinkId = this.GetCurrentTopVotedDrink();
                string setDrinkOfTheDayQuery = "INSERT INTO DrinkOfTheDay (DrinkId, DrinkTime) VALUES (@drinkId, @date);";
                var parameters = new List<SqlParameter>
                {
                    new ("@drinkId", SqlDbType.Int) { Value = drinkId },
                    new ("@date", SqlDbType.DateTime) { Value = DateTime.UtcNow },
                };
                this.dataBaseManager.ExecuteDataModificationQuery(setDrinkOfTheDayQuery, parameters);
            }
            catch (Exception exception)
            {
                throw new Exception("Database error occurred while resetting drink of the day." + exception.Message, exception);
            }
        }

        /// <summary>
        /// Votes for a drink of the day.
        /// </summary>
        /// <param name="userId"> User id. </param>
        /// <param name="drinkId"> Drink id. </param>
        public void VoteDrinkOfTheDay(int userId, int drinkId)
        {
            try
            {
                DateTime voteTime = DateTime.UtcNow;
                string checkAlreadyVotedQuery = "SELECT COUNT(*) as Count FROM Vote WHERE UserId = @userId AND CONVERT(date, VoteTime) = CONVERT(date, @voteTime);";
                var parameters = new List<SqlParameter>
                {
                    new ("@userId", SqlDbType.Int) { Value = userId },
                    new ("@voteTime", SqlDbType.DateTime) { Value = voteTime },
                };
                var selectResult = this.dataBaseManager.ExecuteSelectQuery(checkAlreadyVotedQuery, parameters);
                int voteCount = Convert.ToInt32(selectResult[FirstResult]["Count"]);

                string voteQuery = string.Empty;
                if (voteCount == ZeroResults)
                {
                    voteQuery = "INSERT INTO Vote (UserId, DrinkId, VoteTime) VALUES (@userId, @drinkId, @voteTime);";
                }
                else
                {
                    voteQuery = "UPDATE Vote SET DrinkId = @drinkId WHERE UserId = @userId AND CONVERT(date, VoteTime) = CONVERT(date, @voteTime);";
                }

                var voteParameters = new List<SqlParameter>
                {
                    new ("@userId", SqlDbType.Int) { Value = userId },
                    new ("@drinkId", SqlDbType.Int) { Value = drinkId },
                    new ("@voteTime", SqlDbType.DateTime) { Value = voteTime },
                };
                this.dataBaseManager.ExecuteDataModificationQuery(voteQuery, voteParameters);
            }
            catch (Exception exception)
            {
                throw new Exception("Database error occurred while voting for drink of the day." + exception.Message, exception);
            }
        }

        /// <summary>
        /// Retrieves a list of drinks based on the user's personal drink list.
        /// </summary>
        /// <param name="userId"> User id. </param>
        /// <returns> The list of drinks for the user. </returns>
        public List<Drink> GetPersonalDrinkList(int userId)
        {
            try
            {
                string getPersonalDrinkIdsQuery = "SELECT DrinkId FROM UserDrink WHERE UserId = @userId;";
                var parameters = new List<SqlParameter>
                {
                    new ("@userId", SqlDbType.Int) { Value = userId },
                };
                var selectResult = this.dataBaseManager.ExecuteSelectQuery(getPersonalDrinkIdsQuery, parameters);
                if (selectResult.Count == ZeroResults)
                {
                    return new List<Drink>();
                }

                List<Drink> drinks = new List<Drink>();
                foreach (var drink in selectResult)
                {
                    int drinkId = Convert.ToInt32(drink["DrinkId"]);
                    Drink? drinkDetails = this.GetDrinkById(drinkId);
                    if (drinkDetails != null)
                    {
                        drinks.Add(drinkDetails);
                    }
                }

                return drinks;
            }
            catch (Exception exception)
            {
                throw new Exception("Database error occurred while retrieving personal drink list." + exception.Message, exception);
            }
        }

        /// <summary>
        /// Checks if a drink is already in the user's personal drink list.
        /// </summary>
        /// <param name="userId"> User id. </param>
        /// <param name="drinkId"> Drink id. </param>
        /// <returns> True, if it is in the list, false otherwise. </returns>
        public bool IsDrinkInPersonalList(int userId, int drinkId)
        {
            try
            {
                string checkDrinkQuery = "SELECT COUNT(*) as Count FROM UserDrink WHERE UserId = @userId AND DrinkId = @drinkId;";
                var parameters = new List<SqlParameter>
                {
                    new ("@userId", SqlDbType.Int) { Value = userId },
                    new ("@drinkId", SqlDbType.Int) { Value = drinkId },
                };
                var selectResult = this.dataBaseManager.ExecuteSelectQuery(checkDrinkQuery, parameters);
                if (selectResult.Count == ZeroResults)
                {
                    return false;
                }
                else
                {
                    int drinkCount = Convert.ToInt32(selectResult[FirstResult]["Count"]);
                    return drinkCount > ZeroResults;
                }
            }
            catch (Exception exception)
            {
                throw new Exception("Database error occurred while checking if drink is in personal list." + exception.Message, exception);
            }
        }

        /// <summary>
        /// Adds a drink to the user's personal drink list.
        /// </summary>
        /// <param name="userId"> User id. </param>
        /// <param name="drinkId"> Drink id. </param>
        /// <returns> True, if successfull, false otherwise. </returns>
        public bool AddToPersonalDrinkList(int userId, int drinkId)
        {
            try
            {
                if (this.IsDrinkInPersonalList(userId, drinkId))
                {
                    return true;
                }
                else
                {
                    string addDrinkQuery = "INSERT INTO UserDrink (UserId, DrinkId) VALUES (@userId, @drinkId);";
                    var parameters = new List<SqlParameter>
                    {
                        new ("@userId", SqlDbType.Int) { Value = userId },
                        new ("@drinkId", SqlDbType.Int) { Value = drinkId },
                    };
                    this.dataBaseManager.ExecuteDataModificationQuery(addDrinkQuery, parameters);
                    return true;
                }
            }
            catch (Exception exception)
            {
                throw new Exception("Database error occurred while adding drink to personal list." + exception.Message, exception);
            }
        }

        /// <summary>
        /// Deletes a drink from the user's personal drink list.
        /// </summary>
        /// <param name="userId"> User id. </param>
        /// <param name="drinkId"> Drink id. </param>
        /// <returns> True, if successfull, false otherwise. </returns>
        public bool DeleteFromPersonalDrinkList(int userId, int drinkId)
        {
            try
            {
                string deleteDrinkQuery = "DELETE FROM UserDrink WHERE UserId = @userId AND DrinkId = @drinkId;";
                var parameters = new List<SqlParameter>
                {
                    new ("@userId", SqlDbType.Int) { Value = userId },
                    new ("@drinkId", SqlDbType.Int) { Value = drinkId },
                };
                this.dataBaseManager.ExecuteDataModificationQuery(deleteDrinkQuery, parameters);
                return true;
            }
            catch (Exception exception)
            {
                throw new Exception("Database error occurred while deleting drink from personal list." + exception.Message, exception);
            }
        }

        /// <summary>
        /// Retrieves the current top voted drink.
        /// </summary>
        /// <returns> Id of the current top voted drink. </returns>
        public int GetCurrentTopVotedDrink()
        {
            try
            {
                string getTopVotedDrinkQuery = @"
                SELECT TOP 1 DrinkId, COUNT(*) AS VoteCount
                FROM Vote
                WHERE CONVERT(date, VoteTime) >= CONVERT(date, @VoteTime)
                GROUP BY DrinkId
                ORDER BY COUNT(*) DESC;";

                var parameters = new List<SqlParameter>
                {
                    new ("@VoteTime", SqlDbType.DateTime) { Value = DateTime.UtcNow.Date.AddDays(-1) },
                };

                var selectResult = this.dataBaseManager.ExecuteSelectQuery(getTopVotedDrinkQuery, parameters);
                if (selectResult.Count == ZeroResults)
                {
                    return this.GetRandomDrinkId();
                }

                int drinkId = Convert.ToInt32(selectResult[FirstResult]["DrinkId"]);
                return drinkId;
            }
            catch (Exception exception)
            {
                throw new Exception("Database error occurred while retrieving current top voted drink." + exception.Message, exception);
            }
        }

        /// <summary>
        /// Retrieves a random drink id from the database.
        /// </summary>
        /// <returns> Random drink id. </returns>
        public int GetRandomDrinkId()
        {
            try
            {
                string getRandomDrinkIdQuery = "SELECT TOP 1 DrinkId FROM Drink ORDER BY NEWID();";
                var selectResult = this.dataBaseManager.ExecuteSelectQuery(getRandomDrinkIdQuery);
                if (selectResult.Count == ZeroResults)
                {
                    throw new Exception("No drink found in the database.");
                }

                int randomDrinkId = Convert.ToInt32(selectResult[FirstResult]["DrinkId"]);
                return randomDrinkId;
            }
            catch (Exception exception)
            {
                throw new Exception("Database error occurred while retrieving random drink id." + exception.Message, exception);
            }
        }

        /// <summary>
        /// Retrieves a list of all available drink categories.
        /// </summary>
        /// <returns> List of all categories. </returns>
        public List<Category> GetDrinkCategories()
        {
            List<Category> categories = new List<Category>();

            try
            {
                string getCategoriesQuery = "SELECT * FROM Category ORDER BY CategoryId;";
                var selectResult = this.dataBaseManager.ExecuteSelectQuery(getCategoriesQuery);

                foreach (var row in selectResult)
                {
                    int id = Convert.ToInt32(row["CategoryId"]);
                    string name = row["CategoryName"].ToString();
                    categories.Add(new Category(id, name));
                }

                return categories;
            }
            catch (Exception exception)
            {
                throw new Exception("Database error occurred while retrieving drink categories.", exception);
            }
        }

        /// <summary>
        /// Retrieves a list of all available drink brands.
        /// </summary>
        /// <returns> List of all brands. </returns>
        public List<Brand> GetDrinkBrands()
        {
            try
            {
                string getBrandsQuery = "SELECT * FROM Brand;";
                var selectResult = this.dataBaseManager.ExecuteSelectQuery(getBrandsQuery);
                List<Brand> brands = new List<Brand>();
                foreach (var row in selectResult)
                {
                    int id = Convert.ToInt32(row["BrandId"]);
                    string name = row["BrandName"].ToString();
                    brands.Add(new Brand(id, name));
                }

                return brands;
            }
            catch (Exception exception)
            {
                throw new Exception("Database error occurred while retrieving drink brands.", exception);
            }
        }

        /// <summary>
        /// Retrieves a list of drink categories by drink id.
        /// </summary>
        /// <param name="drinkId"> Id of the drink. </param>
        /// <returns> Categories for the specific drink. </returns>
        public List<Category> GetDrinkCategoriesById(int drinkId)
        {
            try
            {
                string getDrinkCategoryIdsQuery = "SELECT CategoryId FROM DrinkCategory WHERE DrinkId = @drinkId;";
                var categoryIdParameters = new List<SqlParameter>
                {
                    new ("@drinkId", SqlDbType.Int) { Value = drinkId },
                };
                var selectResult = this.dataBaseManager.ExecuteSelectQuery(getDrinkCategoryIdsQuery, categoryIdParameters);
                if (selectResult.Count == ZeroResults)
                {
                    throw new Exception("No drink found with the provided ID.");
                }

                List<Category> categories = new List<Category>();
                foreach (var row in selectResult)
                {
                    int categoryId = Convert.ToInt32(row["CategoryId"]);
                    string getCategoryQuery = "SELECT * FROM Category WHERE CategoryId = @categoryId;";
                    var categoryParameters = new List<SqlParameter>
                    {
                        new ("@categoryId", SqlDbType.Int) { Value = categoryId },
                    };
                    var categorySelectResult = this.dataBaseManager.ExecuteSelectQuery(getCategoryQuery, categoryParameters);
                    if (categorySelectResult.Count == ZeroResults)
                    {
                        throw new Exception("No category found with the provided ID.");
                    }

                    int id = Convert.ToInt32(categorySelectResult[FirstResult]["CategoryId"]);
                    string name = categorySelectResult[FirstResult]["CategoryName"].ToString();
                    categories.Add(new Category(id, name));
                }

                return categories;
            }
            catch (Exception exception)
            {
                throw new Exception("Database error occurred while retrieving drink categories for specific drink." + exception.Message, exception);
            }
        }

        /// <summary>
        /// Retrieves the drink brand for the drink id.
        /// </summary>
        /// <param name="drinkId"> Drink id. </param>
        /// <returns> Brand. </returns>
        public Brand GetBrandById(int drinkId)
        {
            try
            {
                string getDrinkBrandIdQuery = "SELECT BrandId FROM Drink WHERE DrinkId = @drinkId;";
                var parameters = new List<SqlParameter>
                {
                    new ("@drinkId", SqlDbType.Int) { Value = drinkId },
                };
                var selectResult = this.dataBaseManager.ExecuteSelectQuery(getDrinkBrandIdQuery, parameters);
                if (selectResult.Count == ZeroResults)
                {
                    throw new Exception("No drink found with the provided ID.");
                }

                int brandId = Convert.ToInt32(selectResult[FirstResult]["BrandId"]);
                string getBrandQuery = "SELECT * FROM Brand WHERE BrandId = @brandId;";
                var brandParameters = new List<SqlParameter>
                {
                    new ("@brandId", SqlDbType.Int) { Value = brandId },
                };
                var brandSelectResult = this.dataBaseManager.ExecuteSelectQuery(getBrandQuery, brandParameters);
                if (brandSelectResult.Count == ZeroResults)
                {
                    throw new Exception("No brand found with the provided ID.");
                }

                brandId = Convert.ToInt32(brandSelectResult[FirstResult]["BrandId"]);
                string brandName = brandSelectResult[FirstResult]["BrandName"].ToString();

                return new Brand(brandId, brandName);
            }
            catch (Exception exception)
            {
                throw new Exception("Database error occurred while retrieving drink brand for specific drink." + exception.Message, exception);
            }
        }

        /// <summary>
        /// Checks if a drink brand is already in the database.
        /// </summary>
        /// <param name="brandName"> Brand name. </param>
        /// <returns> True, if yes, false otherwise. </returns>
        public bool IsBrandInDatabase(string brandName)
        {
            try
            {
                string checkBrandQuery = "SELECT COUNT(*) as Count FROM Brand WHERE BrandName = @brandName;";
                var parameters = new List<SqlParameter>
                {
                    new ("@brandName", SqlDbType.VarChar) { Value = brandName },
                };
                var selectResult = this.dataBaseManager.ExecuteSelectQuery(checkBrandQuery, parameters);
                if (selectResult.Count == ZeroResults)
                {
                    throw new Exception("No brand found with the provided name.");
                }

                int brandCount = Convert.ToInt32(selectResult[FirstResult]["Count"]);
                return brandCount > ZeroResults;
            }
            catch (Exception exception)
            {
                throw new Exception("Database error occurred while checking if brand is in database." + exception.Message, exception);
            }
        }

        /// <summary>
        /// Adds a new drink brand to the database.
        /// </summary>
        /// <param name="brandName"> Brand name. </param>
        public void AddBrand(string brandName)
        {
            try
            {
                string addBrandQuery = "INSERT INTO Brand (BrandName) VALUES (@brandName);";
                var parameters = new List<SqlParameter>
                {
                    new ("@brandName", SqlDbType.VarChar) { Value = brandName },
                };
                this.dataBaseManager.ExecuteDataModificationQuery(addBrandQuery, parameters);
            }
            catch (Exception exception)
            {
                throw new Exception("Database error occurred while adding a brand." + exception.Message, exception);
            }
        }
    }
}
