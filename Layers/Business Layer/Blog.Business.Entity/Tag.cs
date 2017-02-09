namespace Blog.Business.Entity
{
	using Microsoft.AspNet.Identity;
	using System;
	using System.Collections.Generic;

	public class Tag : Entity
	{ 

		public virtual string Name { get; set; }

		public virtual int State { get; set; }
	}
}