using Capstone.Web.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capstone.Web.Models.ViewModels
{
	public class DashboardViewModel
	{
		public List<Recipe> UserRecipes { get; set; } = new List<Recipe>();
		public List<MealPlan> UserMealPlans { get; set; } = new List<MealPlan>();
		public List<Meal> UserMeals { get; set; } = new List<Meal>();
		public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
		
		//add a tab selection property to select the correct tab or use JS

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dal"></param>
		/// <param name="userId"></param>
		/// <returns></returns>
		public static DashboardViewModel DefaultView(IDatabaseSvc dal, int userId)
		{
			DashboardViewModel model = new DashboardViewModel
			{
				UserRecipes = dal.GetAllRecipes(userId),
				UserMealPlans = dal.GetAllMealPlans(userId),
				UserMeals = dal.GetAllMeals(userId),
				Ingredients = dal.GetAllIngredients()
			};

			return model;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dal"></param>
		/// <param name="mealPlanId"></param>
		/// <returns></returns>
		public static DashboardViewModel MealPlanView(IDatabaseSvc dal, int mealPlanId)
		{
			DashboardViewModel model = new DashboardViewModel();
			List<MealPlan> mealPlans = new List<MealPlan>
			{
				dal.GetMealPlan(mealPlanId)
			};

			model.UserMealPlans = mealPlans;

			List<Meal> meals = dal.GetMealPlanMeals(mealPlanId);

			model.UserMeals = meals;
			
			foreach (var item in meals)
			{
				model.UserRecipes.AddRange(dal.GetMealRecipes(item.Id));
			}
			
			model.Ingredients = dal.GetAllIngredients();

			return model;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dal"></param>
		/// <param name="mealId"></param>
		/// <param name="mealPlanId"></param>
		/// <returns></returns>
		public static DashboardViewModel MealView(IDatabaseSvc dal, int mealId, int userId)
		{
			
			DashboardViewModel model = new DashboardViewModel
			{
				UserMealPlans = dal.GetAllMealPlans(userId),
				UserRecipes = dal.GetMealRecipes(mealId),
				Ingredients = dal.GetAllIngredients()
			};

			model.UserMeals.Add(dal.GetMeal(mealId));
			

			return model;
		}
	}
}