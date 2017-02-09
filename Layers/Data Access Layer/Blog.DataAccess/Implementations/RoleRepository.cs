namespace Blog.DataAccess.Implementations
{
    using Business.Entity;
    using Interfaces;
    using NHibernate.Linq;
    using System.Linq;
    using System.Collections.Generic;

      public class RoleRepository : RepositoryBase<Role>, IRoleRepository
    {
        public Role GetByName(string roleName)
        {
            return Session.Query<Role>().FirstOrDefault(x => x.Name == roleName);         
        }

        public int GetIdRole(string roleName)
        {

            var query = Session.Query<Role>();
            Role role = query.FirstOrDefault(x => x.Name == roleName);

            return (role!= null?role.Id:0);
        }

        /// <summary>
        /// Returns a list of user's roles
        /// </summary>
        /// <param name="idUser">The user's id</param>
        /// <returns></returns>
        public IList<string> GetUserRolesByIdUser(int idUser)
        {
            return Session.CreateSQLQuery("exec GetUserRolesByIdUser :IdUser")
                .SetParameter("IdUser", idUser).List<string>();
        }

        /// <summary>
        /// Deletes all roles from a user in the UserRoles table
        /// </summary>
        /// <param name="idUser">The user's id</param>
        /// <returns></returns>
        public int DeleteUserRolesByIdUser(int idUser)
        {
            return Session.CreateSQLQuery("exec DeleteUserRolesByIdUser :IdUser")
               .SetParameter("IdUser", idUser).ExecuteUpdate();
        }

        /// <summary>
        /// Inserts a new role for a user in the UserRoles table
        /// </summary>
        /// <param name="user">The User</param>
        /// <param name="idRole">The Role's id</param>
        /// <returns></returns>
        public int InsertUserRoles(User user, int idRole)
        {
            return Session.CreateSQLQuery("exec InsertUserRoles :IdUser, :IdRole")
                    .SetParameter("IdUser", user.Id)
                    .SetParameter("IdRole", idRole).ExecuteUpdate();
        }
    }
}