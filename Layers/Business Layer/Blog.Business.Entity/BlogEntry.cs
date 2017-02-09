namespace Blog.Business.Entity
{
	using Microsoft.AspNet.Identity;
	using System;
	using System.Collections.Generic;

	public class BlogEntry : Entity
	{ 

		public virtual string Header { get; set; }

		public virtual string HeaderUrl { get; set; }

		public virtual string Author { get; set; }

		public virtual string ShortContent { get; set; }

		public virtual string Content { get; set; }

		public virtual int State { get; set; }

        public virtual List<Tag> Tags { get; set; }

        public virtual List<BlogEntryComment> Comments { get; set; }
    }
}