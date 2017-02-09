namespace Blog.Business.Logic.Interfaces
{
    using Entity;
    using System.Collections.Generic;

    public interface IBlogEntryCommentBL : IBaseLogic<BlogEntryComment>
    {
        IList<BlogEntryComment> GetByIdBlogEntry(int idBlogEntry);
    }
}