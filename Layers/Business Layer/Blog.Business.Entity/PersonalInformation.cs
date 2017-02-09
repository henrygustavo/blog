namespace Blog.Business.Entity
{
	using Microsoft.AspNet.Identity;
	using System;
	using System.Collections.Generic;

	public class PersonalInformation : Entity
	{ 

		public virtual string FirstName { get; set; }

		public virtual string LastName { get; set; }

		public virtual string SiteName { get; set; }

		public virtual string Email { get; set; }

		public virtual string Country { get; set; }

		public virtual string PhoneNumber { get; set; }

		public virtual string IdPhoto { get; set; }

		public virtual string Description { get; set; }

		public virtual string FaceBook { get; set; }

		public virtual string Twitter { get; set; }

		public virtual string GooglePlus { get; set; }
	}
}