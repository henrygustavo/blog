namespace Blog.DataAccess.Interfaces
{
	using Business.Entity;
	using NHibernate.Linq;
	using System.Linq;
	using System.Collections.Generic;

	public interface ITagRepository : IRepository<Tag>
	{

	}
}
