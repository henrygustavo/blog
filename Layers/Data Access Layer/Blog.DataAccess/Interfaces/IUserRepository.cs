namespace Blog.DataAccess.Interfaces
{
    using Business.Entity;
    using System.Collections.Generic;
    using System.Security.Claims;
    using Microsoft.AspNet.Identity;

    public interface IUserRepository : IRepository<User>
    {
                User GetByUserName(string userName);
        User GetByEmail(string email);
        User GetById(int id);
        string GetPasswordHash(int idUser);
        int UpdatePasswordHash(int idUser, string passwordHash);
        string GetSecurityStamp(int idUser);
        ClaimsIdentity GetUserClaimsByIdUser(int idUser);
        int DeleteUserClaimsByIdUser(int idUser);
        int InsertUserClaims(Claim userClaim, int idUser);
        int DeleteUserClaims(User user, Claim claim);
        int DeleteUserLogins(User user, UserLoginInfo login);
        int DeleteUserLoginsByIdUser(int idUser);
        int InsertUserLogins(User user, UserLoginInfo login);
        int GetIdUserByUserLogins(UserLoginInfo userLogin);
        List<UserLoginInfo> GetUserLoginsByIdUser(int idUser);
        IList<UserView> GetAllView(List<FilterOption> filters, int pageNumber, int pageSize, string sortBy, string sortDirection, List<string> selectColumnsList = null);
        int CountGetAllView(List<FilterOption> filters, int pageNumber, int pageSize);
    }
}