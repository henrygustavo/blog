namespace Blog.WebApiSite.Models
{
	using Business.Entity;
	using System.ComponentModel.DataAnnotations;

	public class TagModel:Tag
	{     
		[Required]
		public override int Id { get; set; }
		[Required]
		public override string Name { get; set; }
		[Required]
		public override int State { get; set; }
	}
}