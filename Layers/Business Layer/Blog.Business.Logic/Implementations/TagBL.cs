namespace Blog.Business.Logic.Implementations
{
	using Entity;
	using Interfaces;
	using DataAccess.Interfaces;
	using System.Collections.Generic;
	using System.Linq;

	public class TagBL : ITagBL
	{
		private readonly ITagRepository _repository;
		private readonly ICommonRepository _commonRepository;

		public TagBL(ITagRepository repository, ICommonRepository commonRepository)
		{
			_repository = repository;
			_commonRepository = commonRepository;
		}

		public Tag Get(int id)
		{
			return _repository.Get(id);
		}

		[UnitOfWork]
		public IList<Tag> GetAll()
		{
			/* Used UnitOfWork attribute, because GetAll method returns IQueryable<Person>, it does not fetches records from database. 
			 * No database operation is performed until ToList(). Thus, we need to control connection open/close in this method using UnitOfWork. */
			/* Used UnitOfWork attribute, because when different repositories and all operations must be transactional. */
			return _repository.GetAll().ToList();
		}

		public List<Tag> GetAll(List<FilterOption> filters, int pageNumber, int pageSize, string sortBy, string sortDirection, List<string> selectColumnsList = null)
		{
			return _repository.GetAll(filters, pageNumber, pageSize, sortBy, sortDirection, selectColumnsList).ToList();
		}

		public int CountGetAll(List<FilterOption> filters, int pageNumber, int pageSize)
		{
			return _repository.CountGetAll(filters, pageNumber, pageSize);
		}

		public void Insert(Tag entity)
		{
		 /* Not used UnitOfWork attribute, because this method only calls one repository method and repoository can manage it's connection/transaction. */
			
			System.DateTime currentDate = _commonRepository.GetCurrentDateTime();
			entity.CreationDate = currentDate;
			entity.LastActivityDate = currentDate;
			entity.LastActivityIdUser = entity.CreationIdUser;

			_repository.Insert(entity);
		}

		public void Update(Tag entity)
		{
		 /* Not used UnitOfWork attribute, because this method only calls one repository method and repoository can manage it's connection/transaction. */
			
			System.DateTime currentDate = _commonRepository.GetCurrentDateTime();
			entity.LastActivityDate = currentDate;
			entity.LastActivityIdUser = entity.LastActivityIdUser;
			_repository.Update(entity);
		}

		public void Delete(int id)
		{
			_repository.Delete(id);
		}	
	}
}