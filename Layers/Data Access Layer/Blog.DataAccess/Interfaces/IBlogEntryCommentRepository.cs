namespace Blog.DataAccess.Interfaces
{
	using Business.Entity;
	using NHibernate.Linq;
	using System.Linq;
	using System.Collections.Generic;

	public interface IBlogEntryCommentRepository : IRepository<BlogEntryComment>
	{
	    IList<BlogEntryComment> GetByIdBlogEntry(int idBlogEntry);

	}
}
