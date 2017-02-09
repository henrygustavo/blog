namespace Blog.DataAccess.Mappings
{
    using Business.Entity;
    using FluentNHibernate.Mapping;


    public class BlogEntryViewMap : ClassMap<BlogEntryView>
    {
        public BlogEntryViewMap()
        {
            Table("dbo.[BlogEntryView]");

            Id(x => x.Id).Column("IdBlogEntry");
        
            Map(x => x.Header);

            Map(x => x.HeaderUrl);

            Map(x => x.Author);

            Map(x => x.State);

            Map(x => x.StateName);

            Map(x => x.CreationDate);

            Map(x => x.Tags);

            Map(x => x.ShortContent);

            Map(x => x.Content);

            Map(x => x.TotalComments);
        }
    }
}
