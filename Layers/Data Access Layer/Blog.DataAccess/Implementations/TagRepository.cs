namespace Blog.DataAccess.Implementations
{
	using Business.Entity;
	using Interfaces;
	using NHibernate.Linq;
	using System.Linq;
	using System.Collections.Generic;
	using NHibernate.Transform;

	public class TagRepository : RepositoryBase<Tag>, ITagRepository
	{

	}
}