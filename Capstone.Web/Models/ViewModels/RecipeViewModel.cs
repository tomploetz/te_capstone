using Capstone.Web.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Capstone.Web.Models.ViewModels
{
	public class RecipeViewModel
	{
		[Required(ErrorMessage = "Recipe name is required")]
		[DataType(DataType.Text)]
		public string Name { get; set; }

		[Required(ErrorMessage = "Prep time is required")]
		[DataType(DataType.Text)]
		public int PrepTime { get; set; }

		[Required(ErrorMessage = "Cook time is required")]
		[DataType(DataType.Text)]
		public int CookTime { get; set; }

		public int TotalTime
		{
			get
			{
				return PrepTime + CookTime;
			}
		}

		[Required(ErrorMessage = "Description is required")]
		[DataType(DataType.Text)]
		public string Description { get; set; }

		public List<SelectListItem> Ingredients { get; set; } = new List<SelectListItem>();

		public List<int> IngredientSelection { get; set; } = new List<int>();

		//we know there should probably be a RecipeDetailViewModel but this is what we have so far
		public List<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();

		[Required(ErrorMessage = "Instructions are required")]
		[DataType(DataType.Text)]
		public string Instructions { get; set; }

		public string Image { get; set; }


		
	}
}