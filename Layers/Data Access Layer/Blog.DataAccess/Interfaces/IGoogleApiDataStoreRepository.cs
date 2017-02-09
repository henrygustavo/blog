namespace Blog.DataAccess.Interfaces
{
	using Business.Entity;

	public interface IGoogleApiDataStoreRepository : IRepository<GoogleApiDataStore>
	{
		GoogleApiDataStore GetByUserName(string userName);
		int TruncateTable();
	}
}
