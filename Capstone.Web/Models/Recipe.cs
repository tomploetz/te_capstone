using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Capstone.Web.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PrepTime { get; set; }
        public int CookTime { get; set; }
        public int TotalTime
        {
            get
            {
                return PrepTime + CookTime;
            }
        }
        public int CategoryId { get; set; }
        public string Description { get; set; }
		public List<Ingredient> Ingredients { get; set; }
		public string Instructions { get; set; }
        public string Image { get; set; }
        public bool Shareable { get; set; }
        public int UserId { get; set; }        
    }
}