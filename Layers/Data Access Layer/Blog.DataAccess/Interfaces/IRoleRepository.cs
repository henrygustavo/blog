namespace Blog.DataAccess.Interfaces
{
    using Business.Entity;
    using System.Collections.Generic;

    public interface IRoleRepository : IRepository<Role>
    {
        Role GetByName(string roleName);
        int GetIdRole(string roleName);
        IList<string> GetUserRolesByIdUser(int idUser);
        int DeleteUserRolesByIdUser(int idUser);
        int InsertUserRoles(User user, int idRole);
    }
}
