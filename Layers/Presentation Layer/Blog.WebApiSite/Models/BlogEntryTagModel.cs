namespace Blog.WebApiSite.Models
{
	using Business.Entity;
	using System.ComponentModel.DataAnnotations;

	public class BlogEntryTagModel:BlogEntryTag
	{     
		[Required]
		public override int Id { get; set; }
		[Required]
		public override int IdBlogEntry { get; set; }
		[Required]
		public override int IdTag { get; set; }
	}
}