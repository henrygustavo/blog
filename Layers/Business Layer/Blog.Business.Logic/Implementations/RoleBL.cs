namespace Blog.Business.Logic.Implementations
{
    using Entity;
    using Interfaces;
    using DataAccess.Interfaces;
    using System.Collections.Generic;
    using System.Linq;

   public class RoleBL : IRoleBL
    {
        private readonly IRoleRepository _repository;

        public RoleBL(IRoleRepository repository)
        {
            _repository = repository;
        }

        public Role Get(int id)
        {
            return _repository.Get(id);
        }

        [UnitOfWork]
        public IList<Role> GetAll()
        {
            return _repository.GetAll().ToList();
        }

        public List<Role> GetAll(List<FilterOption> filters, int pageNumber, int pageSize, string sortBy, string sortDirection, List<string> selectColumnsList = null)
        {
            return _repository.GetAll(filters, pageNumber, pageSize, sortBy, sortDirection, selectColumnsList).ToList();
        }

        public int CountGetAll(List<FilterOption> filters, int pageNumber, int pageSize)
        {
            return _repository.CountGetAll(filters, pageNumber, pageSize);
        }

        public void Insert(Role entity)
        {
            _repository.Insert(entity);
        }

        public void Update(Role entity)
        {
            _repository.Update(entity);
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }

        public Role GetByName(string roleName)
        {
            return _repository.GetByName(roleName);
        }

        public int GetIdRole(string roleName)
        {
            return _repository.GetIdRole(roleName);
        }

        public IList<string> GetUserRolesByIdUser(int idUser)
        {
            return _repository.GetUserRolesByIdUser(idUser);
        }

        public int DeleteUserRolesByIdUser(int idUser)
        {
            return _repository.DeleteUserRolesByIdUser(idUser);
        }

        public int InsertUserRoles(User user, int idRole)
        {
            return _repository.InsertUserRoles(user, idRole);
        }
    }
}