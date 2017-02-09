namespace Blog.DataAccess.Mappings
{
	using Business.Entity;
	using FluentNHibernate.Mapping;

	public class BlogEntryTagMap : ClassMap<BlogEntryTag>
	{
		public BlogEntryTagMap()
		{
			Table("dbo.[BlogEntryTag]");

            Id(x => x.Id).Column("IdBlogEntryTag");

            Map(x => x.IdBlogEntry);

			Map(x => x.IdTag);

			Map(x => x.CreationIdUser);

			Map(x => x.LastActivityIdUser);

			Map(x => x.CreationDate);

			Map(x => x.LastActivityDate);
		}
	}
}