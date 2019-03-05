using Capstone.Web.DAL;
using Capstone.Web.Models;
using Capstone.Web.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Capstone.Web.Controllers
{
    public class RecipeController : Controller
    {
        #region member variable/constructor
        private IDatabaseSvc _db = null;

		public RecipeController(IDatabaseSvc db)
		{
			_db = db;
		}
        #endregion

		// GET: Recipe/Add
		public ActionResult Add(int? recipeId)
		{
			ActionResult result;

			//if no user is logged in, redirect to home page
			if (Session["User"] == null)
			{
				result = RedirectToAction("Index", "Home");
			}
			else
			//else if (recipeId != null)
			//{
			//	Recipe currentRecipe = _db.GetRecipe((int)recipeId);
			//	RecipeViewModel model = new RecipeViewModel
			//	{
			//		Name = currentRecipe.Name,
			//		Description = currentRecipe.Description,
			//		PrepTime = currentRecipe.PrepTime,
			//		CookTime = currentRecipe.CookTime,
			//		Instructions = currentRecipe.Instructions,
			//		Ingredients = GetIngredients()
			//	};
			//	result = View("Add", model);
			//}
			{
				//create a recipeviewmodel and populate it with all ingredients
				RecipeViewModel model = new RecipeViewModel
				{
					Ingredients = GetIngredients()
				};

				result = View("Add", model);
			}

			return result;
		}

        // POST: Recipe/Add
		[HttpPost]
		public ActionResult Add(RecipeViewModel model)
		{
			ActionResult result;
			
			if(!ModelState.IsValid)
			{
				result = View("Add", model);
			}
			else
			{
				//get userid from session to add to recipe
				User currentUser = Session["User"] as User;

				//populate recipe model from recipeviewmodel
				Recipe recipe = new Recipe
				{
					Name = model.Name,
					Description = model.Description,
					PrepTime = model.PrepTime,
					CookTime = model.CookTime,
					Instructions = model.Instructions,
					UserId = currentUser.Id

				};
				//add recipe to db and return recipeid
				int recipeId = _db.AddRecipe(recipe);

				//create recipeid session and assign it new recipe
				Session["RecipeId"] = recipeId;
				
				//loop through selected recipe ingredients and add them to the database
				for (int i = 0; i < model.IngredientSelection.Count; i++)
				{
					RecipeIngredient ri = new RecipeIngredient
					{
						RecipeId = recipeId,
						Id = model.IngredientSelection[i]
					};
                    _db.AddRecipeIngredients(ri.RecipeId, ri.Id);
				}

				result = RedirectToAction("RecipeIngredients"/*, new { recipeId = recipeId }*/);
			}
            return result;
		}

		// GET: Recipe/RecipeIngredients
		public ActionResult RecipeIngredients()
		{
			ActionResult result;

			if (Session["User"] == null)
			{
				result = RedirectToAction("Index", "Home");
			}
			else
			{
				//assign the session recipeid to a variable for use
				int recipeId = (int)Session["RecipeId"];

				//create a list of recipeingredients and get all recipe ingredients based on recipeid
				List<RecipeIngredient> recipeIngredients = _db.GetRecipeIngredients(recipeId);

				//create an empty dictionary for ingredients
				Dictionary<string, RecipeIngredient> ingredients = new Dictionary<string, RecipeIngredient>();

				//loop through recipeIngredients and add the name and ingredient information to the dictionary
				for (int i = 0; i < recipeIngredients.Count; i++)
				{
					ingredients.Add(recipeIngredients[i].Name, recipeIngredients[i]);
				}

				//set ingredients dictionary to the ingredients session
				Session["Ingredients"] = ingredients;

				result = View("RecipeIngredients", ingredients);
			}

			return result;
		}

		// POST: Recipe/RecipeIngredients
		[HttpPost]
		public ActionResult RecipeIngredients(List<int> Quantity, List<string> UnitType, List<string> Name)
		{
            ActionResult result = null;

			//add the session dictionary of recipe ingredients to a new dictionary
			Dictionary<string, RecipeIngredient> recipeIngredients = Session["Ingredients"] as Dictionary<string, RecipeIngredient>;

			//loop through each item in the dictionary and update the database records
			for(int i = 0; i < Name.Count; i++)
			{
				var recipeIngredient = recipeIngredients[Name[i]];
				recipeIngredient.Quantity = Quantity[i];
				recipeIngredient.UnitType = UnitType[i];
				_db.UpdateRecipeIngredient(recipeIngredient);
			}
			
			//destroy recipeid and ingredients sessions after recipe is submitted
			Session["Ingredients"] = null;
			Session["RecipeId"] = null;

			result = RedirectToAction("Dashboard", "User");
			
			return result;
		}

		//GET: Recipe/RecipeDetail
		public ActionResult RecipeDetail(int recipeId)
		{
			ActionResult result;

			if (Session["User"] == null)
			{
				result = RedirectToAction("Index", "Home");
			}
			else
			{
				//create a new recipe model with a recipe based on recipeid provided
				Recipe recipe = _db.GetRecipe(recipeId);

				//create a recipeviewmodel and populate with correct recipe properties
				RecipeViewModel model = new RecipeViewModel
				{
					Name = recipe.Name,
					Description = recipe.Description,
					PrepTime = recipe.PrepTime,
					CookTime = recipe.CookTime,
					Instructions = recipe.Instructions,
					//get all recipeingredients from the database based on recipeid
					RecipeIngredients = _db.GetDetailRecipeIngredients(recipeId)
				};

				result = View(model);
			}

			return result;
		}

		// GET: Recipe/Edit/5
		public ActionResult Edit(int recipeId)
		{
			ActionResult result;

			if (Session["User"] == null)
			{
				result = RedirectToAction("Index", "Home");
			}
			else
			{
				//create a new recipe model and populate with a recipe from the database
				Recipe recipe = _db.GetRecipe(recipeId);

				//assign the session for recipeid with the recipe to edit
				Session["RecipeId"] = recipe.Id;

				//populate a new recipeviewmodel with the recipe properties provided
				RecipeViewModel model = new RecipeViewModel
				{
					Name = recipe.Name,
					Description = recipe.Description,
					PrepTime = recipe.PrepTime,
					CookTime = recipe.CookTime,
					Instructions = recipe.Instructions,
					RecipeIngredients = _db.GetDetailRecipeIngredients(recipeId)
				};

				result = View("Edit", model);
			}

			return result;
		}

		//POST: Recipe/Edit/5
		[HttpPost]
		public ActionResult Edit(RecipeViewModel model)
		{
			ActionResult result;

			Recipe updateRecipe = new Recipe
			{
				Name = model.Name,
				Description = model.Description,
				PrepTime = model.PrepTime,
				CookTime = model.CookTime,
				Instructions = model.Instructions
			};
			//update recipe DAL

			for(int i = 0; i < model.RecipeIngredients.Count; i++)
			{
				_db.UpdateRecipeIngredient(model.RecipeIngredients[i]);
			}

			//int recipeId = (int)Session["RecipeId"];

			result = RedirectToAction("Dashboard", "User");

			return result;
		}

		// POST: Recipe/DeleteRecipe
		[HttpPost]
		public ActionResult DeleteRecipe(int recipeId)
		{
			_db.DeleteRecipe(recipeId);

			return RedirectToAction("Dashboard", "User");
		}

		/// <summary>
		/// method to get all ingredients from the database
		/// </summary>
		/// <returns>a list of ingredients for the user to choose from</returns>
		public List<SelectListItem> GetIngredients()
		{
			List<SelectListItem> selectListIngredients = new List<SelectListItem>();
			List<Ingredient> ingredients = _db.GetAllIngredients();

			foreach (var item in ingredients)
			{
				selectListIngredients.Add(new SelectListItem() { Text = $"{item.Name}", Value = $"{item.Id}" });
			}

			return selectListIngredients;
		}
	}
}