namespace Blog.Business.Logic.Interfaces
{
    using Entity;
    using System.Collections.Generic;

    public interface IRoleBL : IBaseLogic<Role>
    {
        Role GetByName(string roleName);
        int GetIdRole(string roleName);
        IList<string> GetUserRolesByIdUser(int idUser);
        int DeleteUserRolesByIdUser(int idUser);
        int InsertUserRoles(User user, int idRole);
    }
}