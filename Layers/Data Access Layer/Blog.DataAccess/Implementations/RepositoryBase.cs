namespace Blog.DataAccess.Implementations
{
    using System.Collections.Generic;
    using System.Linq;
    using Business.Entity;
    using Interfaces;
    using NHibernate;
    using NHibernate.Criterion;
    using NHibernate.Linq;
    using NHibernate.Transform;

    /// <summary>
    /// Base class for all repositories those uses NHibernate.
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public abstract class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        /// <summary>
        /// Gets the NHibernate session object to perform database operations.
        /// </summary>
        protected ISession Session { get { return UnitOfWork.Current.Session; } }

        /// <summary>
        /// Used to get a IQueryable that is used to retrive object from entire table.
        /// </summary>
        /// <returns>IQueryable to be used to select entities from database</returns>
        public IQueryable<TEntity> GetAll()
        {
            return Session.Query<TEntity>();
        }

        public IList<TEntity> GetAll(List<FilterOption> filters,int pageNumber, int pageSize, string sortBy, string sortDirection,List<string>selectColumnsList = null)
        {
            return GetAllGeneric<TEntity>(filters, pageNumber, pageSize, sortBy, sortDirection, selectColumnsList);
        }

        protected IList<T> GetAllGeneric<T>(List<FilterOption> filters, int pageNumber, int pageSize, string sortBy, string sortDirection, List<string> selectColumnsList = null)
        {
            ICriteria query = GetQueryCondition<T>(filters);

            selectColumnsList = selectColumnsList ?? new List<string>();

            if (selectColumnsList.Any())
            {
                ProjectionList projectionList = Projections.ProjectionList();

                foreach (var column in selectColumnsList)
                {
                    projectionList.Add(Projections.Property(column), column);
                }
                query.SetProjection(projectionList);
            }

            string sortByText = string.Empty;

            var property = typeof(T).GetProperties().SingleOrDefault(prop => prop.Name.ToLower() == sortBy.ToLower());

            if (property != null)
            {
                sortByText = property.Name;
            }
            
            Order order = sortDirection.ToLower() == "asc" ? Order.Asc(sortByText) : Order.Desc(sortByText);

            return  query.SetFirstResult((pageNumber - 1) * pageSize)
                    .SetMaxResults(pageSize).AddOrder(order)
                    .SetResultTransformer(Transformers.AliasToBean(typeof(T)))
                    .List<T>();
        }

        public int CountGetAll(List<FilterOption> filters, int pageNumber, int pageSize)
        {
            return CountGetAllGeneric<TEntity>(filters, pageNumber, pageSize);
        }

        protected int CountGetAllGeneric<T>(List<FilterOption> filters, int pageNumber, int pageSize)
        {
            ICriteria query = GetQueryCondition<T>(filters);

            return query.SetProjection(Projections.RowCount()).UniqueResult<int>();
        }

        protected ICriteria GetQueryCondition<T>(List<FilterOption> filters)
        {
            ICriteria query = Session.CreateCriteria(typeof(T));

            if (filters != null)
            {
                foreach (var filter in filters)
                {
                    if (string.IsNullOrEmpty(filter.Field) || filter.Value == null || string.IsNullOrEmpty(filter.Sign)) continue;

                    switch (filter.Sign)
                    {
                        case "%":

                            query.Add(Restrictions.InsensitiveLike(filter.Field, filter.Value.ToString(), MatchMode.Anywhere));

                            break;

                        case "=":

                            query.Add(Restrictions.Eq(filter.Field, filter.Value));
                            break;

                        case "!=":

                            query.Add(Restrictions.Not(Restrictions.Eq(filter.Field, filter.Value)));
                            break;

                    }
                }
            }

            return query;
        }
        /// <summary>
        /// Gets an entity.
        /// </summary>
        /// <param name="id">Primary key of the entity to get</param>
        /// <returns>Entity</returns>
        public TEntity Get(int id)
        {
            return Session.Get<TEntity>(id);
        }

        /// <summary>
        /// Inserts a new entity.
        /// </summary>
        /// <param name="entity">Entity</param>
        public void Insert(TEntity entity)
        {
            Session.Save(entity);
        }

        /// <summary>
        /// Updates an existing entity.
        /// </summary>
        /// <param name="entity">Entity</param>
        public void Update(TEntity entity)
        {
            Session.Update(entity);
        }

        /// <summary>
        /// Deletes an entity.
        /// </summary>
        /// <param name="id">Id of the entity</param>
        public void Delete(int id)
        {
            Session.Delete(Session.Load<TEntity>(id));
        }
    }
}