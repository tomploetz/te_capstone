using Capstone.Web.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Capstone.Web.Models;
using Capstone.Web.Models.ViewModels;

namespace Capstone.Web.Controllers
{
    public class HomeController : Controller
    {
		private IDatabaseSvc _db = null;

		public HomeController(IDatabaseSvc db)
		{
			_db = db;			
		}

        // GET: Home
        public ActionResult Index()
        {
			return View();
        }

		// GET: Home/About
		public ActionResult About()
		{
			return View();
		}

		// GET: Home/Ingredient
		public ActionResult Ingredient()
        {
			ActionResult result;

			//if no user logged in, redirect to index
			if (Session["User"] == null)
			{
				result = RedirectToAction("Index");
			}
			else
			{
				//populate a list of all ingredients for the view
				List<Ingredient> ingredients = _db.GetAllIngredients();
				ViewBag.Ingredients = ingredients;

				result = View();
			}

			return result;
        }

        // POST: Home/Ingredient
        [HttpPost]
        public ActionResult Ingredient(Ingredient model)
        {
            ActionResult result;
            
            if (!ModelState.IsValid)
            {                
                result = View("Ingredient", model);
            }
            else
            {
				//add ingredient from user input
                _db.AddIngredient(model);

				//redirect to ingredient view
                result = RedirectToAction("Ingredient");
            }

            return result;
        }

		/// <summary>
		/// gets a list of all ingredients for recipe selection
		/// </summary>
		/// <returns>a list of selectlistitems of ingredients with their name and id</returns>
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