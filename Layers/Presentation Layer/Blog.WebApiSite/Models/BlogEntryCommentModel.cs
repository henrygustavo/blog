namespace Blog.WebApiSite.Models
{
	using Business.Entity;
	using System.ComponentModel.DataAnnotations;

	public class BlogEntryCommentModel:BlogEntryComment
	{     
		[Required]
		public override int Id { get; set; }
		[Required]
		public override int IdBlogEntry { get; set; }
		[Required]
		public override string Name { get; set; }
		[Required]
		public override string Comment { get; set; }
		[Required]
		public override string Email { get; set; }
		[Required]
		public override bool AdminPost { get; set; }
	}
}