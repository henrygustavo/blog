namespace Blog.DataAccess.Implementations
{
	using Business.Entity;
	using Interfaces;
	using NHibernate.Linq;
	using System.Linq;
	using System.Collections.Generic;
	using NHibernate.Transform;

	public class GoogleApiDataStoreRepository : RepositoryBase<GoogleApiDataStore>, IGoogleApiDataStoreRepository
	{
        public GoogleApiDataStore GetByUserName(string userName)
        {
            return Session.Query<GoogleApiDataStore>().FirstOrDefault(x => x.UserName == userName);
        }

        public int TruncateTable()
        {
            return Session.CreateSQLQuery("exec TruncateGoogleApiDataStore").ExecuteUpdate();
        }
    }
}