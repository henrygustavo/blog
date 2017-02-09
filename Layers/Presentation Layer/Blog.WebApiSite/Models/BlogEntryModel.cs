namespace Blog.WebApiSite.Models
{
	using Business.Entity;
	using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;

    public class BlogEntryModel:BlogEntry
	{     
		[Required]
		public override int Id { get; set; }
		[Required]
		public override string Header { get; set; }

		[Required]
		public override string Author { get; set; }
		[Required]
		public override string ShortContent { get; set; }
		[Required]
		public override string Content { get; set; }
		[Required]
		public override int State { get; set; }
    }
}