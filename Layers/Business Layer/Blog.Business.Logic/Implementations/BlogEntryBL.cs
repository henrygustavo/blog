namespace Blog.Business.Logic.Implementations
{
	using Entity;
	using Interfaces;
	using DataAccess.Interfaces;
	using System.Collections.Generic;
	using System.Linq;

	public class BlogEntryBL : IBlogEntryBL
	{
		private readonly IBlogEntryRepository _repository;
		private readonly ICommonRepository _commonRepository;
        private readonly IBlogEntryTagRepository _blogEntryTagRepository;
        private readonly ITagRepository _tagRepository;

        public BlogEntryBL(IBlogEntryRepository repository, IBlogEntryTagRepository blogEntryTagRepository, ITagRepository tagRepository, ICommonRepository commonRepository)
		{
			_repository = repository;
			_commonRepository = commonRepository;
            _blogEntryTagRepository = blogEntryTagRepository;
		    _tagRepository = tagRepository;


		}

		public BlogEntry Get(int id)
		{
			return _repository.Get(id);
		}

        public BlogEntry GetByHeaderUrl(string headerUrl)
        {
            return _repository.GetByHeaderUrl(headerUrl);
        }

        public BlogEntry GetByHeader(string header)
        {
            return _repository.GetByHeader(header);
        }

        [UnitOfWork]
		public IList<BlogEntry> GetAll()
		{
			/* Used UnitOfWork attribute, because GetAll method returns IQueryable<Person>, it does not fetches records from database. 
			 * No database operation is performed until ToList(). Thus, we need to control connection open/close in this method using UnitOfWork. */
			/* Used UnitOfWork attribute, because when different repositories and all operations must be transactional. */
			return _repository.GetAll().ToList();
		}

		public List<BlogEntry> GetAll(List<FilterOption> filters, int pageNumber, int pageSize, string sortBy, string sortDirection, List<string> selectColumnsList = null)
		{
			return _repository.GetAll(filters, pageNumber, pageSize, sortBy, sortDirection, selectColumnsList).ToList();
		}

		public int CountGetAll(List<FilterOption> filters, int pageNumber, int pageSize)
		{
			return _repository.CountGetAll(filters, pageNumber, pageSize);
		}

        public IList<BlogEntryView> GetAllView(List<FilterOption> filters, int pageNumber, int pageSize, string sortBy, string sortDirection, List<string> selectColumnsList = null)
        {
            return _repository.GetAllView(filters, pageNumber, pageSize, sortBy, sortDirection, selectColumnsList);
        }

        public int CountGetAllView(List<FilterOption> filters, int pageNumber, int pageSize)
        {
            return _repository.CountGetAllView(filters, pageNumber, pageSize);
        }

        [UnitOfWork]
        public void Insert(BlogEntry entity)
		{
		 /* Not used UnitOfWork attribute, because this method only calls one repository method and repoository can manage it's connection/transaction. */
			
			System.DateTime currentDate = _commonRepository.GetCurrentDateTime();
			entity.CreationDate = currentDate;
			entity.LastActivityDate = currentDate;
			entity.LastActivityIdUser = entity.CreationIdUser;

            _repository.Insert(entity);

            InsertBlogEntryTag(entity);

        }

		public void Update(BlogEntry entity)
		{
		 /* Not used UnitOfWork attribute, because this method only calls one repository method and repoository can manage it's connection/transaction. */
			
			System.DateTime currentDate = _commonRepository.GetCurrentDateTime();
			entity.LastActivityDate = currentDate;
			entity.LastActivityIdUser = entity.LastActivityIdUser;
		    _blogEntryTagRepository.DeleteByIdBlogEntry(entity.Id);

            InsertBlogEntryTag(entity);

            _repository.Update(entity);
		}

		public void Delete(int id)
		{
			_repository.Delete(id);
		}

	    private void InsertBlogEntryTag(BlogEntry entity)
	    {
            if (entity.Tags != null && entity.Tags.Any())
            {
                foreach (var tag in entity.Tags)
                {
                    int idTag = tag.Id;

                    if (idTag == 0)
                    {
                        var newTag = new Tag
                        {

                            Name = tag.Name,
                            CreationDate = entity.CreationDate,
                            LastActivityDate = entity.LastActivityDate,
                            CreationIdUser = entity.Id,
                            LastActivityIdUser = entity.LastActivityIdUser,
                            State = _commonRepository.GetSetting((int)EnumSetting.Active).Id
                        };

                        _tagRepository.Insert(newTag);

                        idTag = newTag.Id;

                    }

                    _blogEntryTagRepository.Insert(new BlogEntryTag
                    {
                        IdBlogEntry = entity.Id,
                        IdTag = idTag,
                        CreationDate = entity.CreationDate,
                        LastActivityDate = entity.LastActivityDate,
                        CreationIdUser = entity.Id,
                        LastActivityIdUser = entity.LastActivityIdUser
                    });
                }
            }
        }
	}
}