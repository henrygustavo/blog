namespace Blog.DataAccess.Mappings
{
	using Business.Entity;
	using FluentNHibernate.Mapping;

	public class GoogleApiDataStoreMap : ClassMap<GoogleApiDataStore>
	{
		public GoogleApiDataStoreMap()
		{
			Table("dbo.[GoogleApiDataStore]");

			Id(x => x.Id).Column("IdGoogleApiDataStore");

			Map(x => x.RefreshToken);

			Map(x => x.UserName);
	
		}
	}
}