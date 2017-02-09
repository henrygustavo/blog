namespace  Blog.DataAccess.Implementations
{
    using Business.Entity;
    using Interfaces;
    using NHibernate.Linq;
    using System.Linq;
    using System.Collections.Generic;
    using System;
    using System.Security.Claims;
    using Microsoft.AspNet.Identity;
    using NHibernate.Transform;

   public class UserRepository : RepositoryBase<User>, IUserRepository
    {
       public User GetByUserName(string userName)
        {
            return Session.Query<User>().FirstOrDefault(x => x.UserName == userName);      
        }
       public User GetByEmail(string email)
       {
           User user = Session.Query<User>().FirstOrDefault(x => x.Email == email);
           return user;
       }

        public User GetById(int id)
        {
            return
                Session.CreateSQLQuery("exec GetUserById :IdUser")
                    .SetParameter("IdUser", id)
                    .SetResultTransformer(Transformers.AliasToBean(typeof (User)))
                    .UniqueResult<User>();
        }

        /// <summary>
        /// Return the user's password hash
        /// </summary>
        /// <param name="idUser">The user's id</param>
        /// <returns></returns>
        public string GetPasswordHash(int idUser)
        {
            var passHash =
                Session.CreateSQLQuery("exec GetPasswordHash :IdUser").SetParameter("IdUser", idUser).UniqueResult();

            return (passHash != null) ? passHash.ToString() : string.Empty;
        }

        /// <summary>
        /// Sets the user's password hash
        /// </summary>
        /// <param name="idUser"></param>
        /// <param name="passwordHash"></param>
        /// <returns></returns>
        public int UpdatePasswordHash(int idUser, string passwordHash)
        {
            return
                Session.CreateSQLQuery("exec UpdatePasswordHash :IdUser, :PasswordHash")
                    .SetParameter("IdUser", idUser)
                    .SetParameter("PasswordHash", passwordHash)
                    .ExecuteUpdate();
        }

        /// <summary>
        /// Returns the user's security stamp
        /// </summary>
        /// <param name="idUser"></param>
        /// <returns></returns>
        public string GetSecurityStamp(int idUser)
        {
            return
                Session.CreateSQLQuery("exec GetSecurityStamp :IdUser")
                    .SetParameter("IdUser", idUser)
                    .UniqueResult()
                    .ToString();
        }

        /// <summary>
        /// Returns a ClaimsIdentity instance given a idUser
        /// </summary>
        /// <param name="idUser">The user's id</param>
        /// <returns></returns>
        public ClaimsIdentity GetUserClaimsByIdUser(int idUser)
        {
            ClaimsIdentity claims = new ClaimsIdentity();

            var rows =
                Session.CreateSQLQuery("exec GetUserClaimsByIdUser :IdUser")
                    .SetParameter("IdUser", idUser)
                    .DynamicList();

            foreach (var row in rows)
            {
                Claim claim = new Claim(row.ClaimType, row.ClaimValue);
                claims.AddClaim(claim);
            }

            return claims;
        }

        /// <summary>
        /// Deletes all claims from a user given a idUser
        /// </summary>
        /// <param name="idUser">The user's id</param>
        /// <returns></returns>
        public int DeleteUserClaimsByIdUser(int idUser)
        {
            return
                Session.CreateSQLQuery("exec DeleteUserClaimsByIdUser :IdUser")
                    .SetParameter("IdUser", idUser)
                    .ExecuteUpdate();
        }

        /// <summary>
        /// Inserts a new claim in UserClaims table
        /// </summary>
        /// <param name="userClaim">User's claim to be added</param>
        /// <param name="idUser">User's id</param>
        /// <returns></returns>
        public int InsertUserClaims(Claim userClaim, int idUser)
        {
            return
                Session.CreateSQLQuery("exec InsertUserClaims :IdUser, :ClaimType, :ClaimValue")
                    .SetParameter("IdUser", idUser)
                    .SetParameter("ClaimType", userClaim.Type)
                    .SetParameter("ClaimValue", userClaim.Value)
                    .ExecuteUpdate();
        }

        /// <summary>
        /// Deletes a claim from a user 
        /// </summary>
        /// <param name="user">The user to have a claim deleted</param>
        /// <param name="claim">A claim to be deleted from user</param>
        /// <returns></returns>
        public int DeleteUserClaims(User user, Claim claim)
        {
            return
                Session.CreateSQLQuery("exec DeleteUserClaims :IdUser, :ClaimType, :ClaimValue")
                    .SetParameter("IdUser", user.Id)
                    .SetParameter("ClaimType", claim.Type)
                    .SetParameter("ClaimValue", claim.Value)
                    .ExecuteUpdate();
        }

        /// <summary>
        /// Deletes a login from a user in the UserLogins table
        /// </summary>
        /// <param name="user">User to have login deleted</param>
        /// <param name="login">Login to be deleted from user</param>
        /// <returns></returns>
        public int DeleteUserLogins(User user, UserLoginInfo login)
        {
            return
                Session.CreateSQLQuery("exec DeleteUserLogins :IdUser, :LoginProvider, :ProviderKey")
                    .SetParameter("IdUser", user.Id)
                    .SetParameter("LoginProvider", login.LoginProvider)
                    .SetParameter("ProviderKey", login.ProviderKey)
                    .ExecuteUpdate();
        }

        /// <summary>
        /// Deletes all Logins from a user in the UserLogins table
        /// </summary>
        /// <param name="idUser">The user's id</param>
        /// <returns></returns>
        public int DeleteUserLoginsByIdUser(int idUser)
        {
            return
                Session.CreateSQLQuery("exec DeleteUserLoginsByIdUser :IdUser, :LoginProvider, :ProviderKey")
                    .SetParameter("IdUser", idUser)
                    .ExecuteUpdate();
        }

        /// <summary>
        /// Inserts a new login in the UserLogins table
        /// </summary>
        /// <param name="user">User to have new login added</param>
        /// <param name="login">Login to be added</param>
        /// <returns></returns>
        public int InsertUserLogins(User user, UserLoginInfo login)
        {
            return
                Session.CreateSQLQuery("exec InsertUserLogins :IdUser, :LoginProvider, :ProviderKey")
                    .SetParameter("IdUser", user.Id)
                    .SetParameter("LoginProvider", login.LoginProvider)
                    .SetParameter("ProviderKey", login.ProviderKey)
                    .ExecuteUpdate();
        }

        /// <summary>
        /// Return a idUser given a user's login
        /// </summary>
        /// <param name="userLogin">The user's login info</param>
        /// <returns></returns>
        public int GetIdUserByUserLogins(UserLoginInfo userLogin)
        {
            var query = Session.CreateSQLQuery("exec GetIdUserByUserLogins :LoginProvider, :ProviderKey")
                .SetParameter("LoginProvider", userLogin.LoginProvider)
                .SetParameter("ProviderKey", userLogin.ProviderKey);

            var result = query.UniqueResult();

            return (result != null) ? Convert.ToInt32(result.ToString()) : 0;
        }

        /// <summary>
        /// Returns a list of user's logins
        /// </summary>
        /// <param name="idUser">The user's id</param>
        /// <returns></returns>
        public List<UserLoginInfo> GetUserLoginsByIdUser(int idUser)
        {
            List<UserLoginInfo> logins = new List<UserLoginInfo>();

            var rows = Session.CreateSQLQuery("exec GetUserLoginsByIdUser :IdUser")
                .SetParameter("IdUser", idUser).DynamicList();

            foreach (var row in rows)
            {
                var login = new UserLoginInfo(row.LoginProvider, row.ProviderKey);
                logins.Add(login);
            }

            return logins;
        }

        public IList<UserView> GetAllView(List<FilterOption> filters, int pageNumber, int pageSize, string sortBy, string sortDirection, List<string> selectColumnsList = null)
        {
            return GetAllGeneric<UserView>(filters, pageNumber, pageSize, sortBy, sortDirection, selectColumnsList);
        }

        public int CountGetAllView(List<FilterOption> filters, int pageNumber, int pageSize)
        {
            return CountGetAllGeneric<UserView>(filters, pageNumber, pageSize);
        }
    }
}