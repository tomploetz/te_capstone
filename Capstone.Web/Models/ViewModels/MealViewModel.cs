using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Capstone.Web.Models.ViewModels
{
    public class MealViewModel
    {
        public int Id { get; set; }

		[Required(ErrorMessage = "Meal name is required")]
		[DataType(DataType.Text)]
		public string Name { get; set; }

		[Required(ErrorMessage = "Description is required")]
		[DataType(DataType.Text)]
		public string Description { get; set; }

        public List<SelectListItem> Recipes { get; set; } = new List<SelectListItem>();

        public List<int> RecipeSelection { get; set; } = new List<int>();

		public List<Recipe> MealRecipes { get; set; } = new List<Recipe>();
    }
}