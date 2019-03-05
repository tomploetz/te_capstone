using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Capstone.Web.DAL;
using Capstone.Web.Models;
using Capstone.Web.Models.ViewModels;

namespace Capstone.Web.Controllers
{
    public class MealController : Controller
    {
        #region member variable/constructor
        private IDatabaseSvc _db = null;

        public MealController(IDatabaseSvc db)
        {
            _db = db;
        }
        #endregion

        // GET: Meal/AddMeal
        public ActionResult AddMeal()
        {
            ActionResult result;

            if(Session["User"] == null)
            {
                result = RedirectToAction("Index", "Home");
            }
            else
            {
                MealViewModel model = new MealViewModel();

				//add the current logged in user to the user object
				User currentUser = Session["User"] as User;

				//user the userid to get recipes for that user
				model.Recipes = GetRecipes(currentUser.Id);

				// return the addmeal view with an empty mealviewmodel
                result = View("AddMeal", model);
            }
            return result;
        }

        //POST: Meal/AddMeal
        [HttpPost]
        public ActionResult AddMeal(MealViewModel model)
        {
            ActionResult result;

            if(!ModelState.IsValid)
            {
                result = View("AddMeal", model);
            }
            else
            {
				User currentUser = Session["User"] as User;

				//populate meal model with properties from the mealviewmodel submitted
				Meal meal = new Meal
                {
                    Name = model.Name,
                    //Id = model.Id,
					UserId = currentUser.Id
                };

				//add meal to db and return mealid
				int mealId = _db.AddMeal(meal);

				//loop through user selected ingredients and add them to the database
				for (int i = 0; i < model.RecipeSelection.Count; i++)
				{
					_db.AddRecipesToAMeal(mealId, model.RecipeSelection[i]);
				}

				result = RedirectToAction("Dashboard", "User");
            }

			return result;
        }

		// GET: Meal/AddMealPlan
		public ActionResult AddMealPlan()
		{
			ActionResult result;

			if (Session["User"] == null)
			{
				result = RedirectToAction("Index", "Home");
			}
			else
			{
				//create an empty mealplanviewmodel
				MealPlanViewModel model = new MealPlanViewModel();

				User currentUser = Session["User"] as User;

				//populate model with all user meals for user selection
				model.Meals = GetMeals(currentUser.Id);

				result = View("AddMealPlan", model);
			}

			return result;
		}

		// POST: Meal/AddMealPlan
		[HttpPost]
		public ActionResult AddMealPlan(MealPlanViewModel model)
		{
			ActionResult result;

			if (!ModelState.IsValid)
			{
				result = View("AddMealPlan", model);
			}
			else
			{
				User currentUser = Session["User"] as User;

				//populate mealplan model with user inputs from mealplanviewmodel
				MealPlan mealPlan = new MealPlan
				{
					UserId = currentUser.Id,
					Name = model.Name,
					//Id = model.Id
				};

				//add mealplan to db and return mealplanid
				int mealPlanId = _db.AddMealPlan(mealPlan);

				//add meals selected by user to the database
				for (int i = 0; i < model.MealSelection.Count; i++)
				{
					_db.AddMealsToMealPlan(mealPlanId, model.MealSelection[i]);
				}
				result = RedirectToAction("Dashboard", "User");
			}

			return result;
		}

		// POST: Meal/DeleteMeal
		[HttpPost]
		public ActionResult DeleteMeal(int mealId)
		{
			//call database to delete the selected meal
			_db.DeleteMeal(mealId);

			return RedirectToAction("Dashboard", "User");
		}

		// POST: Meal/DeleteMealPlan
		[HttpPost]
		public ActionResult DeleteMealPlan(int mealPlanId)
		{
			//call database to delete the selected meal plan
			_db.DeleteMealPlan(mealPlanId);

			return RedirectToAction("Dashboard", "User");
		}

		/// <summary>
		/// gets all recipes that the user has created
		/// </summary>
		/// <param name="userId">uses userId to query the correct recipes</param>
		/// <returns>a list of selectlistitems for the user to select</returns>
		public List<SelectListItem> GetRecipes(int userId)
        {
            List<SelectListItem> selectListRecipes = new List<SelectListItem>();
            List<Recipe> recipes = _db.GetAllRecipes(userId);

            foreach(var item in recipes)
            {
                selectListRecipes.Add(new SelectListItem() { Text = $"{item.Name}", Value = $"{item.Id}" });
            }
            return selectListRecipes;
        }

		/// <summary>
		/// gets all meals that the user has created
		/// </summary>
		/// <param name="userId">uses userId to query the correct meals</param>
		/// <returns>a list of selectlistitems for the user to select</returns>
		public List<SelectListItem> GetMeals(int userId)
		{
			List<SelectListItem> selectListMeals = new List<SelectListItem>();
            List<Meal> meals = _db.GetAllMeals(userId);

            foreach (var item in meals)
            {
                selectListMeals.Add(new SelectListItem() { Text = $"{item.Name}", Value = $"{item.Id}" });
            }
            return selectListMeals;
		}
	}
}