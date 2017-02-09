namespace Blog.Business.Entity
{
	using Microsoft.AspNet.Identity;
	using System;
	using System.Collections.Generic;

	public class BlogEntryTag : Entity
	{ 

		public virtual int IdBlogEntry { get; set; }

		public virtual int IdTag { get; set; }
	}
}