using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Capstone.Web.Models
{
	public class RecipeIngredient : Ingredient
	{
		public int RecipeIngredientId { get; set; }
		public int RecipeId { get; set; }
		public double Quantity { get; set; }
		public string UnitType { get; set; }
		public List<SelectListItem> UnitTypes
		{
			get
			{
				return GetUnitTypes();
			}
		}

		public List<SelectListItem> GetUnitTypes()
		{
			List<SelectListItem> UnitTypes = new List<SelectListItem>();
			var data = new[]
			{
				new SelectListItem() {Text = "Teaspoon", Value = "tsp"},
				new SelectListItem() {Text = "Tablespoon", Value = "tbsp"},
				new SelectListItem() {Text = "Fluid Ounce", Value = "floz"},
				new SelectListItem() {Text = "Cup", Value = "c"},
				new SelectListItem() {Text = "Pint", Value = "pt"},
				new SelectListItem() {Text = "Quart", Value = "qt"},
				new SelectListItem() {Text = "Gallon", Value = "gl"},
				new SelectListItem() {Text = "Pound", Value = "lbs"},
				new SelectListItem() {Text = "Ounce", Value = "oz"},
				new SelectListItem() {Text = "Gram", Value = "g"}
				
			};
			UnitTypes = data.ToList();
			return UnitTypes;
		}


        
	}


}