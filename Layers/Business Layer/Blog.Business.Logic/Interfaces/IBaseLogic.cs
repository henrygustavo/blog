namespace Blog.Business.Logic.Interfaces
{
	using System.Collections.Generic;
	using Entity;

	public interface IBaseLogic<TEntity> where TEntity : class
	{
		/// <summary>
		/// Used to get a IQueryable that is used to retrive entities from entire table.
		/// </summary>
		/// <returns>IQueryable to be used to select entities from database</returns>
		IList<TEntity> GetAll();

		List<TEntity> GetAll(List<FilterOption> filters, int pageNumber, int pageSize, string sortBy, string sortDirection, List<string> selectColumnsList = null);

		int CountGetAll(List<FilterOption> filters, int pageNumber, int pageSize);
		/// <summary>
		/// Gets an entity.
		/// </summary>
		/// <param name="key">Primary key of the entity to get</param>
		/// <returns>Entity</returns>
		TEntity Get(int key);

		/// <summary>
		/// Inserts a new entity.
		/// </summary>
		/// <param name="entity">Entity</param>
		void Insert(TEntity entity);

		/// <summary>
		/// Updates an existing entity.
		/// </summary>
		/// <param name="entity">Entity</param>
		void Update(TEntity entity);

		/// <summary>
		/// Deletes an entity.
		/// </summary>
		/// <param name="id">Id of the entity</param>
		void Delete(int id);
	}
}