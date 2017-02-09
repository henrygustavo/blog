namespace Blog.DataAccess.Mappings
{
	using Business.Entity;
	using FluentNHibernate.Mapping;

	public class TagMap : ClassMap<Tag>
	{
		public TagMap()
		{
			Table("dbo.[Tag]");

			Id(x => x.Id).Column("IdTag");

			Map(x => x.Name);

			Map(x => x.State);

			Map(x => x.CreationIdUser);

			Map(x => x.LastActivityIdUser);

			Map(x => x.CreationDate);

			Map(x => x.LastActivityDate);
		}
	}
}