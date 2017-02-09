namespace Blog.DataAccess.Mappings
{
	using Business.Entity;
	using FluentNHibernate.Mapping;

	public class BlogEntryMap : ClassMap<BlogEntry>
	{
		public BlogEntryMap()
		{
			Table("dbo.[BlogEntry]");

            Id(x => x.Id).Column("IdBlogEntry");

            Map(x => x.Header);

			Map(x => x.HeaderUrl);

			Map(x => x.Author);

			Map(x => x.ShortContent);

			Map(x => x.Content);

			Map(x => x.State);

			Map(x => x.CreationIdUser);

			Map(x => x.LastActivityIdUser);

			Map(x => x.CreationDate);

			Map(x => x.LastActivityDate);
		}
	}
}