namespace Blog.DataAccess.Mappings
{
	using Business.Entity;
	using FluentNHibernate.Mapping;

	public class BlogEntryCommentMap : ClassMap<BlogEntryComment>
	{
		public BlogEntryCommentMap()
		{
			Table("dbo.[BlogEntryComment]");

            Id(x => x.Id).Column("IdBlogEntryComment");

            Map(x => x.Name);

			Map(x => x.Comment);

			Map(x => x.Email);

			Map(x => x.AdminPost);	

			Map(x => x.CreationDate);

			Map(x => x.IdBlogEntry);
		}
	}
}