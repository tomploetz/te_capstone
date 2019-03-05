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
    public class UserController : Controller
    {
		private IDatabaseSvc _db = null;

		public UserController(IDatabaseSvc db)
		{
			_db = db;
		}

		// GET: User/Register
		public ActionResult Register()
        {
            return View();
        }

		// POST: User/Register
		[HttpPost]
		public ActionResult Register(RegistrationViewModel model)
		{
			ActionResult result;

			//Validate the model before proceeding
			if (!ModelState.IsValid)
			{
				result = View("Register", model);
			}
			else
			{
				//populate user object with provided parameters
				User user = new User(model.Password)
				{
					FirstName = model.FirstName,
					LastName = model.LastName,
					UserName = model.UserName,
					RoleId = 1
				};
				//add user to the db
				_db.AddUser(user);

				//set session to the newly registered user
				Session["User"] = _db.GetUser(user.UserName);

				//redirect to user dashboard
				result = RedirectToAction("Dashboard");
			}

			return result;
		}

		// GET: User/Login
		public ActionResult Login()
		{
			return View();
		}

		// POST: User/Login
		[HttpPost]
		public ActionResult Login(LoginViewModel model)
		{
			ActionResult result;

			//Validate the model before proceeding
			if (!ModelState.IsValid)
			{
				result = View("Login", model);
			}
			else
			{
				//if the model is valid, create a record with the user entered username
				User record = _db.GetUser(model.UserName);
				
				//create new user object and generate a hash with the attempted password
				User newPh = new User(model.Password, record.Salt);
				
				//verify if the hashes match
				bool isVerified = newPh.Verify(record.Password);

				if(isVerified)
				{
					Session["User"] = record;

					result = RedirectToAction("Dashboard");
				}
				else
				{
					//TODO: flash message saying password or username are incorrect
					result = RedirectToAction("Login", model);
				}
			}

			return result;
		}

		// GET: User/Dashboard
		public ActionResult Dashboard(int? mealPlanId, int? mealId)
		{
			ActionResult result;

			if(Session["User"] == null)
			{
				result = RedirectToAction("Index", "Home");
			}
			else
			{
                User currentUser = Session["User"] as User;
				DashboardViewModel dashboard = null;
				
				if (mealPlanId != null)
				{
					Session["View"] = null;
					Session["View"] = mealPlanId;
					dashboard = DashboardViewModel.MealPlanView(_db, (int)mealPlanId);
				}
				else if (mealId != null)
				{
					Session["View"] = null;
					Session["View"] = mealId;
					dashboard = DashboardViewModel.MealView(_db, (int)mealId, currentUser.Id);
				}
				else
				{
					dashboard = DashboardViewModel.DefaultView(_db, currentUser.Id);
				}
				
				result = View(dashboard);
			}

			return result;
		}

		// GET: User/GroceryList
		public ActionResult GroceryList(int? mealPlanId, int? mealId, int? recipeId)
		{
			ActionResult result;

			if (Session["User"] == null)
			{
				result = RedirectToAction("Index", "Home");
			}
			else
			{
				List<RecipeIngredient> model = new List<RecipeIngredient>();

				if(mealId != null && mealPlanId == null && recipeId == null)
				{
					model.AddRange(_db.GetRecipeIngredientsForMealGroceryList((int)mealId));
				}
				else if(mealId == null && mealPlanId != null && recipeId == null)
				{
					model.AddRange(_db.GetRecipeIngredientsForMealPlanGroceryList((int)mealPlanId));
				}
				else if(mealId == null && mealPlanId == null && recipeId != null)
				{
					model.AddRange(_db.GetRecipeIngredientsForRecipeGroceryList((int)recipeId));
				}
				
				result = View(model);
			}

			return result;
		}

		// GET: User/Logout
		public ActionResult Logout()
		{
			//destroy user session and redirect to home page
			Session["User"] = null;

			Session.Clear();

			return RedirectToAction("Index", "Home");
		}
    }
}