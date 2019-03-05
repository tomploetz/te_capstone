using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Capstone.Web.Models.ViewModels
{
	public class MealPlanViewModel
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Meal name is required")]
		[DataType(DataType.Text)]
		public string Name { get; set; }

		[Required(ErrorMessage = "Description is required")]
		[DataType(DataType.Text)]
		public string Description { get; set; }

		public List<SelectListItem> Meals { get; set; } = new List<SelectListItem>();

		public List<int> MealSelection { get; set; } = new List<int>();

		public List<Meal> MealPlanMeals { get; set; } = new List<Meal>();
	}
}