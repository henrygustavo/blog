namespace Blog.MemberShip
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Business.Entity;
    using Business.Logic.Interfaces;
    using Microsoft.AspNet.Identity;

    /// <summary>
    /// Class that implements the key ASP.NET Identity role store iterfaces
    /// </summary>
    public class RoleStore<TRole> : IQueryableRoleStore<TRole,int>
        where TRole : Role
    {
        public IRoleBL _roleBL;
      
        public IQueryable<TRole> Roles
        {
            get
            {
                throw new NotImplementedException();
            }
        }


        /// <summary>
        /// Default constructor that initializes a new MySQLDatabase
        /// instance using the Default Connection string
        /// </summary>
        public RoleStore()
        {
           
        }

        /// <summary>
        /// Constructor that takes a MySQLDatabase as argument 
        /// </summary>
        public RoleStore(IRoleBL roleBL)
        {
            _roleBL = roleBL;
        }

        public Task CreateAsync(TRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }

            _roleBL.Insert(role);

            return Task.FromResult<object>(null);
        }

        public Task DeleteAsync(TRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }

            _roleBL.Delete(role.Id);

            return Task.FromResult<Object>(null);
        }

        public Task<TRole> FindByIdAsync(int idRole)
        {
            TRole result = _roleBL.Get(idRole) as TRole;

            return Task.FromResult<TRole>(result);
        }

        public Task<TRole> FindByNameAsync(string roleName)
        {
            TRole result = _roleBL.GetByName(roleName) as TRole;

            return Task.FromResult<TRole>(result);
        }

        public Task UpdateAsync(TRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }

            _roleBL.Update(role);

            return Task.FromResult<Object>(null);
        }

        public void Dispose()
        {
            
        }
    }
}