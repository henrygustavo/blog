namespace Blog.WebApiSite.Models
{
	using Business.Entity;
	using System.ComponentModel.DataAnnotations;

	public class PersonalInformationModel:PersonalInformation
	{     
		[Required]
		public override int Id { get; set; }
		[Required]
		public override string FirstName { get; set; }
		[Required]
		public override string LastName { get; set; }
		[Required]
		public override string SiteName { get; set; }
		[Required]
		public override string Email { get; set; }
		[Required]
		public override string Country { get; set; }
		[Required]
		public override string PhoneNumber { get; set; }
		public override string IdPhoto { get; set; }
		public override string Description { get; set; }
		public override string FaceBook { get; set; }
		public override string Twitter { get; set; }
		public override string GooglePlus { get; set; }
	}
}