using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Capstone.Web.Models;

namespace Capstone.Web.DAL
{
	public class DatabaseSvc : IDatabaseSvc
	{
        #region member variables/constructor
        private string _connectionString;

        public DatabaseSvc(string connectionString)
        {
            _connectionString = connectionString;
        }
        #endregion
        

        #region USER 
        /// <summary>
        /// add user
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Adds a user to the database</returns>
        #region
        public bool AddUser(User model)
        {
            bool wasSuccessful = true;
            //TODO: check if username already exists, if it does, throw an error
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                const string sqlAddUser = "INSERT INTO [User] (FirstName, LastName, UserName, Password, Salt, RoleId) " +
                                          "VALUES (@firstname, @lastname, @username, @password, @salt, @roleid);";

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sqlAddUser;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@firstname", model.FirstName);
                cmd.Parameters.AddWithValue("@lastname", model.LastName);
                cmd.Parameters.AddWithValue("@username", model.UserName);
                cmd.Parameters.AddWithValue("@password", model.Password);
                cmd.Parameters.AddWithValue("@salt", model.Salt);
                cmd.Parameters.AddWithValue("@roleid", model.RoleId);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    wasSuccessful = false;
                    throw new Exception("ERROR! not added to database, Please try again.");
                }
            }
            return wasSuccessful;
        }
        #endregion               

        /// <summary>
        /// get user from db to verify login
        /// </summary>
        /// <param name="username"></param>
        /// <returns>user data from databse to verify/validate login info</returns>
        #region
        public User GetUser(string username)
        {
            User result = new User();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                const string sqlGetUser = "SELECT * FROM [User] " +
                                          "WHERE UserName = @UserName;";

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sqlGetUser;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@UserName", username);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    result = PopulateUserFromReader(reader);
                }
            }
            return result;
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="reader"></param>
		/// <returns></returns>
        private User PopulateUserFromReader(SqlDataReader reader)
        {
                User item = new User()
                {
					Id = Convert.ToInt32(reader["Id"]),
                    FirstName = Convert.ToString(reader["FirstName"]),
                    LastName = Convert.ToString(reader["LastName"]),
                    UserName = Convert.ToString(reader["UserName"]),
                    Password = Convert.ToString(reader["Password"]),
                    Salt = Convert.ToString(reader["Salt"]),
                    RoleId = Convert.ToInt32(reader["RoleId"]),
                };
                return item;
        }
        #endregion
        #endregion


        #region INGREDIENT
        /// <summary>
        /// add an ingredient into database
        /// </summary>
        /// <param name="model"></param>
        /// <returns>bool if ingredient was submitted</returns>
        #region
        public bool AddIngredient(Ingredient model)
        {
            bool wasSuccessful = true;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                const string sqlAddIngredient = "INSERT INTO [Ingredient] (Name) " +
                                            "VALUES (@name);";

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sqlAddIngredient;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@name", model.Name);                

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    wasSuccessful = false;
                    throw new Exception("ERROR! ingredient not added, Please try again.");
                }
            }
            return wasSuccessful;
        }
        #endregion        

        /// <summary>
        /// gets ingredients
        /// </summary>
        /// <returns>List of all ingredients in database</returns>
        #region
        public List<Ingredient> GetAllIngredients()
        {
            List<Ingredient> result = new List<Ingredient>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                const string sqlGetAllIngredients = "SELECT * FROM [Ingredient] ";                                          

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sqlGetAllIngredients;
                cmd.Connection = connection;
                
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    result.Add(PopulateIngredientFromReader(reader));
                }
            }
            return result;
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="reader"></param>
		/// <returns></returns>
        private Ingredient PopulateIngredientFromReader(SqlDataReader reader)
        {
            Ingredient item = new Ingredient()
            {
                Name = Convert.ToString(reader["Name"]),
                Id = Convert.ToInt32(reader["Id"]),
            };
            return item;
        }
        #endregion
        #endregion


        #region RECIPE
        /// <summary>
        /// add Recipe into database
        /// </summary>
        /// <param name="model"></param>
        /// <returns>bool if Recipe was submitted</returns>
        #region
        public int AddRecipe(Recipe model)
        {            
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                const string sqlAddRecipe = "INSERT INTO [Recipe] (Name, Prep_Time, Cook_Time, Description, Instructions, User_Id) " +
                                            "VALUES (@name, @preptime, @cooktime, @description, @instructions, @userid); " +
                                            "SELECT CAST(scope_identity() AS int);";

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sqlAddRecipe;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@name", model.Name);
                cmd.Parameters.AddWithValue("@preptime", model.PrepTime);
                cmd.Parameters.AddWithValue("@cooktime", model.CookTime);
                cmd.Parameters.AddWithValue("@description", model.Description);
                cmd.Parameters.AddWithValue("@instructions", model.Instructions);
                cmd.Parameters.AddWithValue("@userid", model.UserId);
                //changed database to accept nulls for test change later

                int recipeId = (int)cmd.ExecuteScalar();

                return recipeId;
            }            
        }
        #endregion

        /// <summary>
        /// gets single recipe
        /// </summary>
        /// <param name="id"></param>
        /// <returns>a recipe object based on input id</returns>
        #region
        public Recipe GetRecipe(int id)
        {
            Recipe recipe = new Recipe();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM recipe WHERE recipe.Id = @Id", conn);

                cmd.Parameters.AddWithValue("@Id", id);
                SqlDataReader reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    recipe.Id = Convert.ToInt32(reader["Id"]);
                    recipe.Name = Convert.ToString(reader["Name"]);
                    recipe.PrepTime = Convert.ToInt32(reader["Prep_Time"]);
                    recipe.CookTime = Convert.ToInt32(reader["Cook_Time"]);
                    recipe.Description = Convert.ToString(reader["Description"]);
                    recipe.Instructions = Convert.ToString(reader["Instructions"]);
                    recipe.Shareable = Convert.ToBoolean(reader["Shareable"]);
                    recipe.UserId = Convert.ToInt32(reader["User_Id"]);
                }
                return recipe;
            }
        }
        #endregion

        /// <summary>
        /// gets all recipes from db for selection in meal using a userId
        /// </summary>
        /// <returns>List of all recipes in database for selection in meal</returns>
        #region
        public List<Recipe> GetAllRecipes(int userId)
        {
            List<Recipe> result = new List<Recipe>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                const string sqlGetRecipes = "SELECT * FROM [Recipe] " +
											 "WHERE User_Id = @userId;";

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sqlGetRecipes;
                cmd.Connection = connection;
				cmd.Parameters.AddWithValue("@userId", userId);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    result.Add(PopulateRecipeFromReader(reader));
                }
            }
            return result;
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="reader"></param>
		/// <returns></returns>
		private Recipe PopulateRecipeFromReader(SqlDataReader reader)
		{
			Recipe item = new Recipe()
			{
				Name = Convert.ToString(reader["Name"]),
				Id = Convert.ToInt32(reader["Id"]),
				PrepTime = Convert.ToInt32(reader["Prep_Time"]),
				CookTime = Convert.ToInt32(reader["Cook_Time"]),
				Description = Convert.ToString(reader["Description"]),
				Instructions = Convert.ToString(reader["Instructions"]),
				//Shareable = Convert.ToBoolean(reader["Shareable"]),
				//Image = Convert.ToString(reader["Image"]),
				//CategoryId = Convert.ToInt32(reader["Category_Id"]),
				UserId = Convert.ToInt32(reader["User_Id"]),
			};
			return item;
		}
		#endregion		
        #endregion


        #region RECIPE_INGREDIENT
        /// <summary>
        /// adds ing id and rec id to rec_ing table
        /// </summary>
        /// <param name="recipeId"></param>
        /// <param name="ingredientId"></param>
        /// <returns>bool rows affected determining if update was successful</returns>
        #region
        public bool AddRecipeIngredients(int recipeId, int ingredientId)
        {
            bool wasSuccessful = true;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
				const string addRecipeIngredients = "INSERT INTO Recipe_Ingredient (Ingredient_Id, Recipe_Id)" +
												"VALUES (@ingredientId, @recipeId); ";

				SqlCommand command = new SqlCommand();
				command.CommandText = addRecipeIngredients;
				command.Connection = conn;
                command.Parameters.AddWithValue("@ingredientId", ingredientId);
                command.Parameters.AddWithValue("@recipeId", recipeId);
                
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    wasSuccessful = false;
                    throw new Exception("Error, review not saved");
                }
            }
            return wasSuccessful;
        }
        #endregion

        /// <summary>
        /// gets recipe ingredients for editing qty and unit types
        /// </summary>
        /// <param name="recipeId"></param>
        /// <returns>list of recipes</returns>
        #region
        public List<RecipeIngredient> GetRecipeIngredients(int recipeId)
        {
            List<RecipeIngredient> result = new List<RecipeIngredient>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
				const string sqlGetRecipeIngredients = "SELECT Recipe_Ingredient.Id, Ingredient.Name " +
												 "AS Ingredient_Name " +
												 "FROM Recipe_Ingredient " +
												 "JOIN Ingredient ON Recipe_Ingredient.Ingredient_Id = Ingredient.Id " +
												 "WHERE Recipe_Id = @recipeId;";

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sqlGetRecipeIngredients;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@recipeId", recipeId);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    result.Add(PopulateRecipeIngredientFromReader(reader));
                }
            }
            return result;
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="recipeId"></param>
		/// <returns></returns>
		public List<RecipeIngredient> GetDetailRecipeIngredients(int recipeId)
		{
			List<RecipeIngredient> result = new List<RecipeIngredient>();

			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				const string sqlGetDetailRecipeIngredients = "SELECT Recipe_Ingredient.Id, Ingredient.Name AS Ingredient_Name, Ingredient_Id, Recipe_Ingredient.Quantity, Recipe_Ingredient.Unit_Type, Recipe_Ingredient.Recipe_Id " +
															 "FROM Recipe_Ingredient " +
															 "JOIN Ingredient ON Recipe_Ingredient.Ingredient_Id = Ingredient.Id " +
															 "WHERE Recipe_Id = @recipeId;";

				SqlCommand cmd = new SqlCommand();
				cmd.CommandText = sqlGetDetailRecipeIngredients;
				cmd.Connection = connection;
				cmd.Parameters.AddWithValue("@recipeId", recipeId);

				SqlDataReader reader = cmd.ExecuteReader();

				while (reader.Read())
				{
					result.Add(PopulateDetailRecipeIngredientFromReader(reader));
				}
			}
			return result;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="reader"></param>
		/// <returns></returns>
		private RecipeIngredient PopulateRecipeIngredientFromReader(SqlDataReader reader)
        {
            RecipeIngredient item = new RecipeIngredient()
            {
                Name = Convert.ToString(reader["Ingredient_Name"]),
                Id = Convert.ToInt32(reader["Id"]),
                //RecipeIngredientId = Convert.ToInt32(reader["Ingredient_Id"]),
                //Quantity = Convert.ToInt32(reader["Recipe_Ingredient.Quantity"]),
                //UnitType = Convert.ToString(reader["Recipe_Ingredient.Unit_Type"]),
                //RecipeId = Convert.ToInt32(reader["Recipe_Id"])
            };
            return item;
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="reader"></param>
		/// <returns></returns>
		private RecipeIngredient PopulateDetailRecipeIngredientFromReader(SqlDataReader reader)
		{
			RecipeIngredient item = new RecipeIngredient()
			{
				Name = Convert.ToString(reader["Ingredient_Name"]),
				Id = Convert.ToInt32(reader["Id"]),
				RecipeIngredientId = Convert.ToInt32(reader["Ingredient_Id"]),
				Quantity = Convert.ToInt32(reader["Quantity"]),
				UnitType = Convert.ToString(reader["Unit_Type"]),
				RecipeId = Convert.ToInt32(reader["Recipe_Id"])
			};
			return item;
		}
		#endregion

		/// <summary>
		/// update recipe_ingredient db with qty and unit of measurement
		/// </summary>
		/// <param name="recipeId"></param>
		/// <param name="ingredientId"></param>
		/// <returns>bool rows affected determining if update was successful</returns>
		#region
		public bool UpdateRecipeIngredient(RecipeIngredient ingredient)
        {
            bool wasSuccessful = true;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
				const string sqlUpdateRecipeIngredients = "UPDATE Recipe_Ingredient " +
														  "SET Quantity = @quantity, Unit_Type = @unitType " +
												          "WHERE Id = @recipeIngredientId; ";

				SqlCommand command = new SqlCommand();
				command.CommandText = sqlUpdateRecipeIngredients;
				command.Connection = conn;
                command.Parameters.AddWithValue("@recipeIngredientId", ingredient.Id);
                command.Parameters.AddWithValue("@quantity", ingredient.Quantity);
                command.Parameters.AddWithValue("@unitType", ingredient.UnitType);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    wasSuccessful = false;
                    throw new Exception("Error, recipe ingredient not saved");
                }
            }
            return wasSuccessful;
        }
        #endregion
        #endregion


        #region MEAL
        /// <summary>
        /// adds recipes to a meal
        /// </summary>
        /// <param name="model"></param>
        /// <returns>bool rows affected determining if update was successful</returns>
        #region
        public bool AddRecipesToAMeal(int mealId, int recipeId)
        {
            bool wasSuccessful = true;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
				const string sqlAddRecipesToAMeal = "INSERT INTO Meal_Recipe (Meal_Id, Recipe_Id) " +
											        "VALUES (@mealId, @recipeId);";

				SqlCommand command = new SqlCommand();
				command.CommandText = sqlAddRecipesToAMeal;
				command.Connection = conn;
                command.Parameters.AddWithValue("@mealId", mealId);
                command.Parameters.AddWithValue("@recipeId", recipeId);

                int rowsAffected = command.ExecuteNonQuery();

                if(rowsAffected == 0)
                {
                    wasSuccessful = false;
                    throw new Exception("Error, recipes not added");
                }
            }
            return wasSuccessful;
        }
        #endregion

        /// <summary>
        /// gets all meals from db for selection in meal plan
        /// </summary>
        /// <returns>list of all meals in db</returns>
        #region
        public List<Meal> GetAllMeals(int userId)
        {
            List<Meal> result = new List<Meal>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                const string sqlGetMeals = "SELECT * FROM [Meal] " +
										   "WHERE User_Id = @userId;";

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sqlGetMeals;
                cmd.Connection = connection;
				cmd.Parameters.AddWithValue("@userId", userId);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    result.Add(PopulateMealFromReader(reader));
                }
            }
            return result;
        }
        #endregion

        /// <summary>
        /// gets a single meal by mealId
        /// </summary>
        /// <param name="mealId"></param>
        /// <returns>a meal</returns>
        #region
        public Meal GetMeal(int mealId)
		{
			Meal meal = new Meal();

			using (SqlConnection conn = new SqlConnection(_connectionString))
			{
				conn.Open();
				const string sqlGetMeal = "SELECT * FROM Meal WHERE Id = @Id;";

				SqlCommand cmd = new SqlCommand();
				cmd.CommandText = sqlGetMeal;
				cmd.Connection = conn;
				cmd.Parameters.AddWithValue("@Id", mealId);
				SqlDataReader reader = cmd.ExecuteReader();

				while (reader.Read())
				{
					meal = PopulateMealFromReader(reader);
				}
				return meal;
			}
		}
        #endregion

        /// <summary>
        /// get recipes in a meal by mealId
        /// </summary>
        /// <param name="mealId"></param>
        /// <returns>List of recipes</returns>
        #region
        public List<Recipe> GetMealRecipes(int mealId)
		{
			List<Recipe> result = new List<Recipe>();

			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				const string sqlGetMealRecipes = "SELECT Recipe.Id, Recipe.Name, Recipe.Prep_Time, Recipe.Cook_Time, Recipe.Description, Recipe.Instructions, Recipe.User_Id " +
												 "FROM Recipe " +
												 "JOIN Meal_Recipe ON Recipe.Id = Meal_Recipe.Recipe_Id " +
												 "WHERE Meal_Recipe.Meal_Id = @mealId;";

				SqlCommand cmd = new SqlCommand();
				cmd.CommandText = sqlGetMealRecipes;
				cmd.Connection = connection;
				cmd.Parameters.AddWithValue("@mealId", mealId);

				SqlDataReader reader = cmd.ExecuteReader();

				while (reader.Read())
				{
					result.Add(PopulateRecipeFromReader(reader));
				}
			}
			return result;
		}
        #endregion

        /// <summary>
        /// populates meal object with info from db
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>meal</returns>
        #region
        private Meal PopulateMealFromReader(SqlDataReader reader)
        {
            Meal item = new Meal()
            {
                Name = Convert.ToString(reader["Name"]),
                Id = Convert.ToInt32(reader["Id"]),
                UserId = Convert.ToInt32(reader["User_Id"]),
            };
            return item;
        }
        #endregion

        /// <summary>
        /// add meal to db
        /// </summary>
        /// <param name="model"></param>
        /// <returns>meal id after one is created when meal is inserted into db</returns>
        #region
        public int AddMeal(Meal model)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                const string sqlAddMeal = "INSERT INTO [Meal] (Name, User_Id) " +
                                          "VALUES (@name, @userid); " +
                                          "SELECT CAST(scope_identity() AS int);";

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sqlAddMeal;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@name", model.Name);
                cmd.Parameters.AddWithValue("@userId", model.UserId);

                int mealId = (int)cmd.ExecuteScalar();

                return mealId;
            }
        }
        #endregion
        #endregion


        #region MEAL PLAN
        /// <summary>
        /// add meals to meal plan
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns>bool base on success of entering into db</returns>
        #region
        public bool AddMealsToMealPlan(int mealPlanId, int mealId)
        {
            bool wasSuccessful = true;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
				const string sqlAddMealsToAMealPlan = "INSERT INTO MealPlan_Meal (MealPlan_Id, Meal_Id) " +
												      "VALUES (@mealPlanId, @mealId);";

				SqlCommand command = new SqlCommand();
				command.CommandText = sqlAddMealsToAMealPlan;
				command.Connection = conn;
                command.Parameters.AddWithValue("@mealPlanId", mealPlanId);
                command.Parameters.AddWithValue(@"mealId", mealId);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    wasSuccessful = false;
                    throw new Exception("Error, recipes not added");
                }
            }
            return wasSuccessful;
        }
        #endregion      
        
        /// <summary>
        /// add meal plan to db
        /// </summary>
        /// <returns>meal plan id after it is created in db</returns>
        #region
        public int AddMealPlan(MealPlan model)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                const string sqlAddMealPlan = "INSERT INTO [Meal_Plan] (Name, User_Id) " +
                                              "VALUES (@name, @userid); " +
                                              "SELECT CAST(scope_identity() AS int);";

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sqlAddMealPlan;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@name", model.Name);
                cmd.Parameters.AddWithValue("@userId", model.UserId);

                int mealPlanId = (int)cmd.ExecuteScalar();

                return mealPlanId;
            }
        }
		#endregion

		/// <summary>
		/// gets meals in a meal plan using mealId
		/// </summary>
		/// <param name="mealPlanId"></param>
		/// <returns>list of meals</returns>
        #region
		public List<Meal> GetMealPlanMeals(int mealPlanId)
		{
			List<Meal> result = new List<Meal>();

			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				const string sqlGetMeals = "SELECT Meal.Id, Meal.Name, Meal.User_Id " +
										   "FROM Meal " +
										   "JOIN MealPlan_Meal " +
										   "ON Meal.Id = MealPlan_Meal.Meal_Id " +
										   "WHERE MealPlan_Meal.MealPlan_Id = @mealPlanId;";

				SqlCommand cmd = new SqlCommand();
				cmd.CommandText = sqlGetMeals;
				cmd.Connection = connection;
				cmd.Parameters.AddWithValue("@mealPlanId", mealPlanId);

				SqlDataReader reader = cmd.ExecuteReader();

				while (reader.Read())
				{
					result.Add(PopulateMealFromReader(reader));
				}
			}
			return result;
		}
        #endregion

        /// <summary>
        /// gets all mealplans for dashboard display
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>list of mealplans</returns>
        #region
        public List<MealPlan> GetAllMealPlans(int userId)
        {
            List<MealPlan> result = new List<MealPlan>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                const string sqlGetMealPlans = "SELECT * FROM [Meal_Plan] " +
                                               "WHERE User_Id = @userId;";

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sqlGetMealPlans;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@userId", userId);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    result.Add(PopulateMealPlanFromReader(reader));
                }
            }
            return result;
        }
        #endregion

        /// <summary>
        /// get meal plan based on meal plan Id
        /// </summary>
        /// <param name="mealPlanId"></param>
        /// <returns>a meal plan</returns>
        #region
        public MealPlan GetMealPlan(int mealPlanId)
		{
			MealPlan mealPlan = new MealPlan();

			using (SqlConnection conn = new SqlConnection(_connectionString))
			{
				conn.Open();
				const string sqlGetMealPlan = "SELECT * FROM Meal_Plan WHERE Id = @Id;";

				SqlCommand cmd = new SqlCommand();
				cmd.CommandText = sqlGetMealPlan;
				cmd.Connection = conn;
				cmd.Parameters.AddWithValue("@Id", mealPlanId);

				SqlDataReader reader = cmd.ExecuteReader();

				while (reader.Read())
				{
					mealPlan = PopulateMealPlanFromReader(reader);					
				}
				return mealPlan;
			}
		}
        #endregion

        /// <summary>
        /// populates meal plan with data from db
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>meal plan</returns>
        #region
        private MealPlan PopulateMealPlanFromReader(SqlDataReader reader)
        {
            MealPlan item = new MealPlan()
            {
                Name = Convert.ToString(reader["Name"]),
                Id = Convert.ToInt32(reader["Id"]),
                UserId = Convert.ToInt32(reader["User_Id"]),
                //Recipes = Convert.ToInt32(reader["Recipe_Id"]),
            };
            return item;
        }
        #endregion
        #endregion        


        #region DELETE METHODS
        /// <summary>
        /// deletes a recipe from db
        /// </summary>
        /// <param name="recipeId"></param>
        /// <returns>bool if delete was successful</returns>
        #region
        public bool DeleteRecipe(int recipeId)
        {
            bool wasSuccessful = true;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                const string sqlDeleteRecipe = "DELETE FROM recipe " +
                                               "WHERE id = @recipeId; ";

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sqlDeleteRecipe;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@recipeId", recipeId);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    wasSuccessful = false;
                    throw new Exception("Error, recipe was not deleted");
                }
            }
            return wasSuccessful;
        }
        #endregion

        /// <summary>
        /// deletes a meal from db
        /// </summary>
        /// <param name="mealId"></param>
        /// <returns>bool if delete was successful</returns>
        #region
        public bool DeleteMeal(int mealId)
        {
            bool wasSuccessful = true;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                const string sqlDeleteMeal = "Delete from meal " +
                                               "Where id = @mealId; ";

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sqlDeleteMeal;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@mealId", mealId);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    wasSuccessful = false;
                    throw new Exception("Error, meal was not deleted");
                }
            }
            return wasSuccessful;
        }
        #endregion

        /// <summary>
        /// deletes a meal plan from db
        /// </summary>
        /// <param name="mealPlanId"></param>
        /// <returns>bool if delete was successful</returns>
        #region
        public bool DeleteMealPlan(int mealPlanId)
        {
            bool wasSuccessful = true;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                const string sqlDeleteMealPlan = "DELETE FROM meal_plan " +
                                                 "WHERE id = @mealPlanId; ";

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sqlDeleteMealPlan;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@mealPlanId", mealPlanId);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    wasSuccessful = false;
                    throw new Exception("Error, meal plan was not deleted");
                }
            }
            return wasSuccessful;
        }
        #endregion
        #endregion


        #region GROCERY
        /// <summary>
        /// gets info for recipe grocery list from db
        /// </summary>
        /// <param name="recipeId"></param>
        /// <returns>list of recipe Ingredients</returns>
        #region
        public List<RecipeIngredient> GetRecipeIngredientsForRecipeGroceryList(int recipeId)
        {
            List<RecipeIngredient> result = new List<RecipeIngredient>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                const string sqlGetRecipeIngredientsForRecipeGroceryList = "SELECT sum(quantity) as totalQuantity, Ingredient.name, Recipe_Ingredient.Unit_Type " +
                                                                           "FROM Recipe_Ingredient " +
                                                                           "JOIN Ingredient ON Recipe_Ingredient.Ingredient_Id = Ingredient.Id " +
                                                                           "WHERE recipe_Id = @recipeId " +
                                                                           "group by recipe_ingredient.Unit_Type, ingredient.name " +
                                                                           "order by ingredient.name;";

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sqlGetRecipeIngredientsForRecipeGroceryList;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@recipeId", recipeId);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    result.Add(PopulateRecipeIngredientsForGroceryListFromReader(reader));
                }
            }
            return result;
        }
        #endregion       

        /// <summary>
        /// gets info for meal grocery list
        /// </summary>
        /// <param name="mealId"></param>
        /// <returns>list of recipe Ingredients</returns>
        #region
        public List<RecipeIngredient> GetRecipeIngredientsForMealGroceryList(int mealId)
        {
            List<RecipeIngredient> result = new List<RecipeIngredient>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                const string sqlGetRecipeIngredientsForMealGroceryList = "SELECT sum(quantity) as totalQuantity, Ingredient.name, Recipe_Ingredient.Unit_Type " +
                                                                         "FROM Recipe_Ingredient " +
                                                                         "JOIN Ingredient ON Recipe_Ingredient.Ingredient_Id = Ingredient.Id " +
                                                                         "Join Meal_Recipe ON recipe_ingredient.recipe_Id = meal_recipe.recipe_Id " +
                                                                         "Join Meal on meal_recipe.meal_Id = meal.Id " +
                                                                         "WHERE Meal_Id = @mealId " +
                                                                         "group by recipe_ingredient.Unit_Type, ingredient.name " +
                                                                         "order by ingredient.name;";

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sqlGetRecipeIngredientsForMealGroceryList;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@mealId", mealId);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    result.Add(PopulateRecipeIngredientsForGroceryListFromReader(reader));
                }
            }
            return result;
        }
        #endregion  

        /// <summary>
        /// gets info for meal plan grocery list
        /// </summary>
        /// <param name="mealPlanId"></param>
        /// <returns>list of recipe Ingredients</returns>
        #region
        public List<RecipeIngredient> GetRecipeIngredientsForMealPlanGroceryList(int mealPlanId)
        {
            List<RecipeIngredient> result = new List<RecipeIngredient>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                const string sqlGetRecipeIngredientsForMealPlanGroceryList = "SELECT sum(quantity) as totalQuantity, Ingredient.name, Recipe_Ingredient.Unit_Type " +
                                                                             "FROM Recipe_Ingredient " +
                                                                             "JOIN Ingredient ON Recipe_Ingredient.Ingredient_Id = Ingredient.Id " +
                                                                             "Join Meal_Recipe ON recipe_ingredient.recipe_Id = meal_recipe.recipe_Id " +
                                                                             "Join Meal on meal_recipe.meal_Id = meal.Id " +
                                                                             "Join MealPlan_Meal on meal.Id = MealPlan_Meal.meal_Id " +
                                                                             "Join Meal_Plan on MealPlan_Meal.MealPlan_Id = Meal_Plan.Id " +
                                                                             "WHERE MealPlan_Id = @mealPlanId " +
                                                                             "group by recipe_ingredient.Unit_Type, ingredient.name " +
                                                                             "order by ingredient.name;";

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sqlGetRecipeIngredientsForMealPlanGroceryList;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@mealPlanId", mealPlanId);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    result.Add(PopulateRecipeIngredientsForGroceryListFromReader(reader));
                }
            }
            return result;
        }
        #endregion

        /// <summary>
        /// populates object with info from db
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>recipe ingredient</returns>
        #region
        private RecipeIngredient PopulateRecipeIngredientsForGroceryListFromReader(SqlDataReader reader)
        {
            RecipeIngredient item = new RecipeIngredient()
            {
                Name = Convert.ToString(reader["Name"]),
                Quantity = Convert.ToInt32(reader["totalQuantity"]),
                UnitType = Convert.ToString(reader["Unit_Type"]),
            };
            return item;
        }
        #endregion
        #endregion
    }
}