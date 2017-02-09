namespace Blog.DataAccess.Interfaces
{
	using Business.Entity;
	using System.Collections.Generic;

	public interface IBlogEntryRepository : IRepository<BlogEntry>
	{
	    BlogEntry GetByHeaderUrl(string headerUrl);
	    BlogEntry GetByHeader(string header);
        IList<BlogEntryView> GetAllView(List<FilterOption> filters, int pageNumber, int pageSize, string sortBy, string sortDirection, List<string> selectColumnsList = null);
        int CountGetAllView(List<FilterOption> filters, int pageNumber, int pageSize);
    }
}
