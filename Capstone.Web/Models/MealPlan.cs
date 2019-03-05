using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capstone.Web.Models
{
    public class MealPlan
    {
        public int Id { get; set; }
        public string Name { get; set; }
		public List<Meal> Meals { get; set; }
        public int UserId { get; set; }
    }
}