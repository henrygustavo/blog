namespace Blog.DataAccess.Interfaces
{
	using Business.Entity;
	using NHibernate.Linq;
	using System.Linq;
	using System.Collections.Generic;

	public interface IBlogEntryTagRepository : IRepository<BlogEntryTag>
	{
	    IList<Tag> GetByIdBlogEntry(int idBlogEntry);

	    int DeleteByIdBlogEntry(int idBlogEntry);

	}
}
