using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capstone.Web.Models
{
	public class Role : BaseItem
	{
		public string RoleName { get; set; }

		public Role Clone()
		{
			Role role = new Role();
			role.Id = this.Id;
			role.RoleName = this.RoleName;
			return role;
		}
	}
}