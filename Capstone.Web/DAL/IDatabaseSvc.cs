using Capstone.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Web.DAL
{
	public interface IDatabaseSvc
	{
		//user
		bool AddUser(User model);
		User GetUser(string username);

        //ingredient
        bool AddIngredient(Ingredient model);
        List<Ingredient> GetAllIngredients();

		//recipe
		int AddRecipe(Recipe model);
        Recipe GetRecipe(int id);
        bool AddRecipeIngredients(int recipeId, int ingredientId);
        List<RecipeIngredient> GetRecipeIngredients(int recipeId);
        bool UpdateRecipeIngredient(RecipeIngredient ingredient);
		List<RecipeIngredient> GetDetailRecipeIngredients(int recipeId);

		//meal
		int AddMeal(Meal model);
        bool AddRecipesToAMeal(int mealId, int recipeId);
        List<Recipe> GetAllRecipes(int userId);
		Meal GetMeal(int mealId);
		List<Meal> GetAllMeals(int userId);
		List<Recipe> GetMealRecipes(int mealId);

		//meal plan
		int AddMealPlan(MealPlan model);
		bool AddMealsToMealPlan(int mealPlanId, int mealId);
        List<MealPlan> GetAllMealPlans(int userId);
		List<Meal> GetMealPlanMeals(int mealPlanId);
		MealPlan GetMealPlan(int mealPlanId);
        
        //delete
        bool DeleteRecipe(int recipeId);
        bool DeleteMeal(int mealId);
        bool DeleteMealPlan(int mealPlanId);

		//grocery lists
		List<RecipeIngredient> GetRecipeIngredientsForRecipeGroceryList(int recipeId);
		List<RecipeIngredient> GetRecipeIngredientsForMealGroceryList(int mealId);
		List<RecipeIngredient> GetRecipeIngredientsForMealPlanGroceryList(int mealPlanId);

	}
}
