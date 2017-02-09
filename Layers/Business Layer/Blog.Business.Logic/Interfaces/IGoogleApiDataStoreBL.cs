namespace Blog.Business.Logic.Interfaces
{
    using Entity;
    using System.Collections.Generic;

    public interface IGoogleApiDataStoreBL : IBaseLogic<GoogleApiDataStore>
    {
        GoogleApiDataStore GetByUserName(string userName);
        int TruncateTable();
    }
}