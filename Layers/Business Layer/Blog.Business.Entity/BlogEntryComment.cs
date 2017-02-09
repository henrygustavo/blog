namespace Blog.Business.Entity
{
	using Microsoft.AspNet.Identity;
	using System;
	using System.Collections.Generic;

	public class BlogEntryComment : Entity
	{ 

		public virtual int IdBlogEntry { get; set; }

		public virtual string Name { get; set; }

		public virtual string Comment { get; set; }

		public virtual string Email { get; set; }

		public virtual bool AdminPost { get; set; }
	}
}