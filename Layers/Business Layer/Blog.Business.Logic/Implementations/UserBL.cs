namespace Blog.Business.Logic.Implementations
{
	using Entity;
	using Interfaces;
	using DataAccess.Interfaces;
	using System.Collections.Generic;
	using System.Linq;
	using System;
	using System.Security.Claims;
	using Microsoft.AspNet.Identity;

	public class UserBL : IUserBL
	{
		private readonly IUserRepository _repository;
		private readonly ICommonRepository _commonRepository;

		public UserBL(IUserRepository repository, ICommonRepository commonRepository)
		{
			_repository = repository;
			_commonRepository = commonRepository;
		}

		public User Get(int id)
		{
			return _repository.Get(id);
		}

		public User GetById(int id)
		{
			return _repository.GetById(id);
		}

		[UnitOfWork]
		public IList<User> GetAll()
		{
			return _repository.GetAll().ToList();
		}

		public List<User> GetAll(List<FilterOption> filters, int pageNumber, int pageSize, string sortBy, string sortDirection, List<string> selectColumnsList = null)
		{
			return _repository.GetAll(filters, pageNumber, pageSize, sortBy, sortDirection, selectColumnsList).ToList();
		}

		public int CountGetAll(List<FilterOption> filters, int pageNumber, int pageSize)
		{
			return _repository.CountGetAll(filters, pageNumber, pageSize);
		}

		public void Insert(User entity)
		{
			DateTime currentDatetTime = _commonRepository.GetCurrentDateTime();

			entity.CreationDate = currentDatetTime;
			entity.LastActivityDate = currentDatetTime;

			_repository.Insert(entity);
		}

		public void Update(User entity)
		{
			DateTime currentDatetTime = _commonRepository.GetCurrentDateTime();
			entity.LastActivityDate = currentDatetTime;
			_repository.Update(entity);
		}

		public void Delete(int id)
		{
			_repository.Delete(id);
		}

		public User GetByUserName(string userName)
		{
			return _repository.GetByUserName(userName);
		}

		public User GetByEmail(string email)
		{
			return _repository.GetByEmail(email);
		}

		public string GetPasswordHash(int idUser)
		{
			return _repository.GetPasswordHash(idUser);
		}

		public int UpdatePasswordHash(int idUser, string passwordHash)
		{
			return _repository.UpdatePasswordHash(idUser, passwordHash);
		}

		public string GetSecurityStamp(int idUser)
		{
			return _repository.GetSecurityStamp(idUser);
		}

		public ClaimsIdentity GetUserClaimsByIdUser(int idUser)
		{
			return _repository.GetUserClaimsByIdUser(idUser);
		}

		public int DeleteUserClaimsByIdUser(int idUser)
		{
			return _repository.DeleteUserClaimsByIdUser(idUser);
		}

		public int InsertUserClaims(Claim userClaim, int idUser)
		{
			return _repository.InsertUserClaims(userClaim, idUser);
		}

		public int DeleteUserClaims(User user, Claim claim)
		{
			return _repository.DeleteUserClaims(user, claim);
		}

		public int DeleteUserLogins(User user, UserLoginInfo login)
		{
			return _repository.DeleteUserLogins(user, login);
		}

		public int DeleteUserLoginsByIdUser(int idUser)
		{
			return _repository.DeleteUserLoginsByIdUser(idUser);
		}

		public int InsertUserLogins(User user, UserLoginInfo login)
		{
			return _repository.InsertUserLogins(user, login);
		}

		public int GetIdUserByUserLogins(UserLoginInfo userLogin)
		{
			return _repository.GetIdUserByUserLogins(userLogin);
		}

		public List<UserLoginInfo> GetUserLoginsByIdUser(int idUser)
		{
			return _repository.GetUserLoginsByIdUser(idUser);
		}

		public List<UserView> GetAllView(List<FilterOption> filters, int pageNumber, int pageSize, string sortBy, string sortDirection, List<string> selectColumnsList = null)
		{
			return _repository.GetAllView(filters, pageNumber, pageSize, sortBy, sortDirection, selectColumnsList).ToList();
		}

		public int CountGetAllView(List<FilterOption> filters, int pageNumber, int pageSize)
		{
			return _repository.CountGetAllView(filters, pageNumber, pageSize);
		}
	}
}