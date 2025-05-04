//// <copyright file="drinkservice.cs" company="placeholdercompany">
//// copyright (c) placeholdercompany. all rights reserved.
//// </copyright>

//namespace winuiapp.services
//{
//    using system;
//    using system.collections.generic;
//    using system.linq;
//    using winuiapp.data.data;
//    using winuiapp.repositories;

//    /// <summary>
//    /// drink service for managing drink-related operations.
//    /// </summary>
//    public class drinkservice : idrinkservice
//    {
//        private const int defaultpersonaldrinkcount = 1;
//        private idrinkrepository drinkrepository;

//        /// <summary>
//        /// initializes a new instance of the <see cref="drinkservice"/> class.
//        /// </summary>
//        public drinkservice()
//        {
//            this.drinkrepository = new drinkrepository();
//        }

//        /// <summary>
//        /// initializes a new instance of the <see cref="drinkservice"/> class with a specified drink model.
//        /// </summary>
//        /// <param name="drinkrepository"> drink repository. </param>
//        public drinkservice(idrinkrepository drinkrepository)
//        {
//            this.drinkrepository = drinkrepository;
//        }

//        /// <summary>
//        /// gets the drink by id.
/// </summary>
//        /// <param name="drinkid"> drink id. </param>
//        /// <returns> the drink. </returns>
//        /// <exception cref="exception"> any issues. </exception>
//        public drink? getdrinkbyid(int drinkid)
//        {
//            try
//            {
//                return this.drinkrepository.getdrinkbyid(drinkid);
//            }
//            catch (exception drinkretrievalexception)
//            {
//                throw new exception($"error happened while getting drink with id {drinkid}:", drinkretrievalexception);
//            }
//        }

//        /// <summary>
//        /// gets drinks based on various filters and ordering criteria.
//        /// </summary>
//        /// <param name="searchkeyword"> search term. </param>
//        /// <param name="drinkbrandnamefilter"> brand filter. </param>
//        /// <param name="drinkcategoryfilter"> category filter. </param>
//        /// <param name="minimumalcoholpercentage"> minimum alcohol content. </param>
//        /// <param name="maximumalcoholpercentage"> maximum alcohol content. </param>
//        /// <param name="orderingcriteria"> order criteria. </param>
//        /// <returns> list of drinks. </returns>
//        /// <exception cref="exception"> any issues. </exception>
//        public list<drink> getdrinks(string? searchkeyword, list<string>? drinkbrandnamefilter, list<string>? drinkcategoryfilter, float? minimumalcoholpercentage, float? maximumalcoholpercentage, dictionary<string, bool>? orderingcriteria)
//        {
//            try
//            {
//                list<drink> alldrinks = this.drinkrepository.getdrinks();
//                list<drink> filtereddrinks = alldrinks;

//                if (minimumalcoholpercentage != null)
//                {
//                    filtereddrinks = filtereddrinks.findall(drink => drink.alcoholcontent >= minimumalcoholpercentage);
//                }

//                if (maximumalcoholpercentage != null)
//                {
//                    filtereddrinks = filtereddrinks.findall(drink => drink.alcoholcontent <= maximumalcoholpercentage);
//                }

//                if (searchkeyword != null && searchkeyword != string.empty)
//                {
//                    filtereddrinks = filtereddrinks.findall(drink => drink.drinkname.tolower().contains(searchkeyword.tolower()));
//                }

//                if (drinkbrandnamefilter != null && drinkbrandnamefilter.count > 0)
//                {
//                    filtereddrinks = filtereddrinks.findall(drink => drinkbrandnamefilter.contains(drink.drinkbrand.brandname));
//                }

//                if (drinkcategoryfilter != null && drinkcategoryfilter.count > 0)
//                {
//                    filtereddrinks = filtereddrinks.findall(drink =>
//                        drinkcategoryfilter.trueforall(categoryfilter =>
//                            drink.categorylist.any(category => category.categoryname == categoryfilter)));
//                }

//                if (orderingcriteria != null && orderingcriteria.count == 1)
//                {
//                    string orderingkey = orderingcriteria.keys.first();
//                    bool isascending = orderingcriteria[orderingkey];

//                    if (orderingkey == "drinkname")
//                    {
//                        if (isascending)
//                        {
//                            filtereddrinks = filtereddrinks.orderby(drink => drink.drinkname).tolist();
//                        }
//                        else
//                        {
//                            filtereddrinks = filtereddrinks.orderbydescending(drink => drink.drinkname).tolist();
//                        }
//                    }
//                    else if (orderingkey == "alcoholcontent")
//                    {
//                        if (isascending)
//                        {
//                            filtereddrinks = filtereddrinks.orderby(drink => drink.alcoholcontent).tolist();
//                        }
//                        else
//                        {
//                            filtereddrinks = filtereddrinks.orderbydescending(drink => drink.alcoholcontent).tolist();
//                        }
//                    }
//                }

//                return filtereddrinks;
//            }
//            catch (exception drinksretrievalexception)
//            {
//                throw new exception("error happened while getting drinks:", drinksretrievalexception);
//            }
//        }

//        /// <summary>
//        /// adds a drink to the database.
//        /// </summary>
//        /// <param name="inputteddrinkname"> name. </param>
//        /// <param name="inputteddrinkpath"> imagepath. </param>
//        /// <param name="inputteddrinkcategories"> categories. </param>
//        /// <param name="inputteddrinkbrandname"> brand. </param>
//        /// <param name="inputtedalcoholpercentage"> alcohol. </param>
//        /// <exception cref="exception"> any issues. </exception>
//        public void adddrink(string inputteddrinkname, string inputteddrinkpath, list<category> inputteddrinkcategories, string inputteddrinkbrandname, float inputtedalcoholpercentage)
//        {
//            try
//            {
//                this.drinkrepository.adddrink(inputteddrinkname, inputteddrinkpath, inputteddrinkcategories, inputteddrinkbrandname, inputtedalcoholpercentage);
//            }
//            catch (exception addingdrinkexception)
//            {
//                throw new exception("error happened while adding a drink:", addingdrinkexception);
//            }
//        }

//        /// <summary>
//        /// updates a drink in the database.
//        /// </summary>
//        /// <param name="drink"> drink. </param>
//        /// <exception cref="exception"> any issues. </exception>
//        public void updatedrink(drink drink)
//        {
//            try
//            {
//                this.drinkrepository.updatedrink(drink);
//            }
//            catch (exception updatedrinkexception)
//            {
//                throw new exception("error happened while updating a drink:", updatedrinkexception);
//            }
//        }

//        /// <summary>
//        /// deletes a drink from the database.
//        /// </summary>
//        /// <param name="drinkid"> drink id. </param>
//        /// <exception cref="exception"> any issues. </exception>
//        public void deletedrink(int drinkid)
//        {
//            try
//            {
//                this.drinkrepository.deletedrink(drinkid);
//            }
//            catch (exception deletedrinkexception)
//            {
//                throw new exception("error happened while deleting a drink:", deletedrinkexception);
//            }
//        }

//        /// <summary>
//        /// retrieves a random drink id from the database.
//        /// </summary>
//        /// <returns> list of categories. </returns>
//        /// <exception cref="exception"> any issues. </exception>
//        public list<category> getdrinkcategories()
//        {
//            try
//            {
//                return this.drinkrepository.getdrinkcategories();
//            }
//            catch (exception drinkcategoriesretrievalexception)
//            {
//                throw new exception("error happened while getting drink categories:", drinkcategoriesretrievalexception);
//            }
//        }

//        /// <summary>
//        /// retrieves a list of drink brands.
//        /// </summary>
//        /// <returns> list of brands. </returns>
//        /// <exception cref="exception"> any issues. </exception>
//        public list<brand> getdrinkbrandnames()
//        {
//            try
//            {
//                return this.drinkrepository.getdrinkbrands();
//            }
//            catch (exception drinkbrandnamesretrievalexception)
//            {
//                throw new exception("error happened while getting drink brands:", drinkbrandnamesretrievalexception);
//            }
//        }

//        /// <summary>
//        /// retrieves a random drink id from the database.
//        /// </summary>
//        /// <param name="userid"> user id. </param>
//        /// <param name="maximumdrinkcount"> not sure. </param>
//        /// <returns> personal list. </returns>
//        /// <exception cref="exception"> any issues. </exception>
//        public list<drink> getuserpersonaldrinklist(int userid, int maximumdrinkcount = defaultpersonaldrinkcount)
//        {
//            try
//            {
//                return this.drinkrepository.getpersonaldrinklist(userid);
//            }
//            catch (exception personaldrinklistretrievalexception)
//            {
//                throw new exception("error getting personal drink list:", personaldrinklistretrievalexception);
//            }
//        }

//        /// <summary>
//        /// checks if a drink is already in the user's personal drink list.
//        /// </summary>
//        /// <param name="userid"> user id. </param>
//        /// <param name="drinkid"> drink id. </param>
//        /// <returns> true, if yes, false otherwise. </returns>
//        /// <exception cref="exception"> any issues. </exception>
//        public bool isdrinkinuserpersonallist(int userid, int drinkid)
//        {
//            try
//            {
//                return this.drinkrepository.isdrinkinpersonallist(userid, drinkid);
//            }
//            catch (exception checkinguserpersonallistexception)
//            {
//                throw new exception("error checking if the drink is in the user's personal list.", checkinguserpersonallistexception);
//            }
//        }

//        /// <summary>
//        /// adds a drink to the user's personal drink list.
//        /// </summary>
//        /// <param name="userid"> user id. </param>
//        /// <param name="drinkid"> drink id. </param>
//        /// <returns> true, if successfull, false otherwise. </returns>
//        /// <exception cref="exception"> any issues. </exception>
//        public bool addtouserpersonaldrinklist(int userid, int drinkid)
//        {
//            try
//            {
//                return this.drinkrepository.addtopersonaldrinklist(userid, drinkid);
//            }
//            catch (exception adddrinktouserpersonallistexception)
//            {
//                throw new exception("error adding drink to personal list:", adddrinktouserpersonallistexception);
//            }
//        }

//        /// <summary>
//        /// deletes a drink from the user's personal drink list.
//        /// </summary>
//        /// <param name="userid"> user id. </param>
//        /// <param name="drinkid"> drink id. </param>
//        /// <returns> true, if successfull, false otherwise. </returns>
//        /// <exception cref="exception"> any issues. </exception>
//        public bool deletefromuserpersonaldrinklist(int userid, int drinkid)
//        {
//            try
//            {
//                return this.drinkrepository.deletefrompersonaldrinklist(userid, drinkid);
//            }
//            catch (exception deletefromuserpersonaldrinklistexception)
//            {
//                throw new exception("error deleting drink from personal list:", deletefromuserpersonaldrinklistexception);
//            }
//        }

//        /// <summary>
//        /// votes for the drink of the day.
//        /// </summary>
//        /// <param name="userid"> user id. </param>
//        /// <param name="drinkid"> drink id. </param>
//        /// <returns> the drink. </returns>
//        /// <exception cref="exception"> any issues. </exception>
//        public drink votedrinkoftheday(int userid, int drinkid)
//        {
//            try
//            {
//                this.drinkrepository.votedrinkoftheday(userid, drinkid);
//                return this.drinkrepository.getdrinkbyid(drinkid) ?? throw new exception("drink not found after voting.");
//            }
//            catch (exception votedrinkofthedayexception)
//            {
//                throw new exception("error voting drink:", votedrinkofthedayexception);
//            }
//        }

//        /// <summary>
//        /// retrieves the drink of the day.
//        /// </summary>
//        /// <returns> the drink. </returns>
//        /// <exception cref="exception"> any issues. </exception>
//        public drink getdrinkoftheday()
//        {
//            try
//            {
//                return this.drinkrepository.getdrinkoftheday();
//            }
//            catch (exception e)
//            {
//                throw new exception("error getting drink of the day:" + e.message, e);
//            }
//        }
//    }
//}