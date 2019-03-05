using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Capstone.Web.Models.ViewModels
{
	public class LoginViewModel
	{
		[Required(ErrorMessage = "Username is required")]
		[DataType(DataType.Text)]
		public string UserName { get; set; }

		[Required]
		[MinLength(8, ErrorMessage = "Password must be 8 characters or more.")]
		[DataType(DataType.Password)]
		public string Password { get; set; }
	}
}