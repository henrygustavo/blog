namespace Blog.Business.Logic.Interfaces
{
    using Entity;
    using System.Collections.Generic;

    public interface IBlogEntryBL : IBaseLogic<BlogEntry>
    {
        BlogEntry GetByHeaderUrl(string headerUrl);

        BlogEntry GetByHeader(string header);

        IList<BlogEntryView> GetAllView(List<FilterOption> filters, int pageNumber, int pageSize, string sortBy, string sortDirection, List<string> selectColumnsList = null);
        int CountGetAllView(List<FilterOption> filters, int pageNumber, int pageSize);
    }
}