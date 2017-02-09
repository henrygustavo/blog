namespace Blog.DataAccess.Implementations
{
	using Business.Entity;
	using Interfaces;
	using NHibernate.Linq;
	using System.Linq;
	using System.Collections.Generic;
	using NHibernate.Transform;

	public class BlogEntryCommentRepository : RepositoryBase<BlogEntryComment>, IBlogEntryCommentRepository
	{
        public IList<BlogEntryComment> GetByIdBlogEntry(int idBlogEntry)
        {
            return Session.Query<BlogEntryComment>().Where(x => x.IdBlogEntry == idBlogEntry).ToList();
        }
    }
}