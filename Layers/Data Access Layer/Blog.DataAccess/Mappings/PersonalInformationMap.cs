namespace Blog.DataAccess.Mappings
{
	using Business.Entity;
	using FluentNHibernate.Mapping;

	public class PersonalInformationMap : ClassMap<PersonalInformation>
	{
		public PersonalInformationMap()
		{
			Table("dbo.[PersonalInformation]");

			Id(x => x.Id).Column("IdPersonalInformation");

			Map(x => x.FirstName);

			Map(x => x.LastName);

			Map(x => x.SiteName);

			Map(x => x.Email);

			Map(x => x.Country);

			Map(x => x.PhoneNumber);

			Map(x => x.IdPhoto);

			Map(x => x.Description);

			Map(x => x.FaceBook);

			Map(x => x.Twitter);

			Map(x => x.GooglePlus);

			Map(x => x.CreationIdUser);

			Map(x => x.LastActivityIdUser);

			Map(x => x.CreationDate);

			Map(x => x.LastActivityDate);
		}
	}
}