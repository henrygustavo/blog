namespace Blog.Business.Entity
{
	using Microsoft.AspNet.Identity;
	using System;
	using System.Collections.Generic;

	public class GoogleApiDataStore : Entity
	{ 

		public virtual string RefreshToken { get; set; }

		public virtual string UserName { get; set; }
	
	}
}