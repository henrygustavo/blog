namespace Blog.Business.Logic.Interfaces
{
    using Entity;
    using System.Collections.Generic;

    public interface IBlogEntryTagBL : IBaseLogic<BlogEntryTag>
    {
        IList<Tag> GetByIdBlogEntry(int idBlogEntry);
    }
}