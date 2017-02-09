namespace Blog.DataAccess.Implementations
{
	using Business.Entity;
	using Interfaces;
	using NHibernate.Linq;
	using System.Linq;
	using System.Collections.Generic;
	using NHibernate.Transform;

	public class BlogEntryTagRepository : RepositoryBase<BlogEntryTag>, IBlogEntryTagRepository
	{
        public IList<Tag> GetByIdBlogEntry(int idBlogEntry)
        {
            return
                    Session.CreateSQLQuery("exec GetBlogEntryTagByIdBlogEntry :IdBlogEntry")
                    .SetParameter("IdBlogEntry", idBlogEntry)
                    .SetResultTransformer(Transformers.AliasToBean(typeof(Tag)))
                    .List<Tag>();
        }

        public int DeleteByIdBlogEntry(int idBlogEntry)
        {
            return
                Session.CreateSQLQuery("exec DeleteBlogEntryTagByIdBlogEntry :IdBlogEntry")
                    .SetParameter("IdBlogEntry", idBlogEntry)
                    .ExecuteUpdate();
        }
    }
}