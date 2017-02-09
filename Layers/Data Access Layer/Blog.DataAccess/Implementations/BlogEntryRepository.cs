namespace Blog.DataAccess.Implementations
{
	using Business.Entity;
	using Interfaces;
	using System.Collections.Generic;
    using System.Linq;
    using NHibernate.Linq;

    public class BlogEntryRepository : RepositoryBase<BlogEntry>, IBlogEntryRepository
	{
        public BlogEntry GetByHeaderUrl(string headerUrl)
        {
            return Session.Query<BlogEntry>().FirstOrDefault(x => x.HeaderUrl == headerUrl);
        }

        public BlogEntry GetByHeader(string header)
        {
            return Session.Query<BlogEntry>().FirstOrDefault(x => x.Header == header);
        }

        public IList<BlogEntryView> GetAllView(List<FilterOption> filters, int pageNumber, int pageSize, string sortBy, string sortDirection, List<string> selectColumnsList = null)
        {
            return GetAllGeneric<BlogEntryView>(filters, pageNumber, pageSize, sortBy, sortDirection, selectColumnsList);
        }

        public int CountGetAllView(List<FilterOption> filters, int pageNumber, int pageSize)
        {
            return CountGetAllGeneric<BlogEntryView>(filters, pageNumber, pageSize);
        }
    }
}