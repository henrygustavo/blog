namespace Blog.WebApiSite.Controllers
{
	using System;
	using System.Web.Http;
	using Business.Logic.Interfaces;
	using Business.Entity;
	using Models;
	using Common.Logging;
	using Microsoft.AspNet.Identity;
	using System.Security.Claims;
	using System.Collections.Generic;
	using System.Linq;
	using Core;
	using System.Threading.Tasks;

	[Authorize(Roles = "admin")]
    [RoutePrefix("api/users")]
    public class UsersController : BaseController
    {          
        private readonly ILog _logger;
        private readonly ApplicationUserManager _appUserManager;
        private readonly IRoleBL _roleBL;
        private readonly IUserBL _userBL;
        public UsersController(ILog logger, ApplicationUserManager appUserManager, IUserBL userBL,IRoleBL roleBL)
            : base(logger)
        {
            _logger = logger;
            _appUserManager = appUserManager;
            _userBL = userBL;
            _roleBL = roleBL;
        }

        public IHttpActionResult GetUsers(string filterName, string filterEmail, int page = 1, int pageSize = 10, string sortBy = "Id", string sortDirection = "asc")
        {
            try
            {
                _logger.Debug(string.Format("ini process - GetAll,idUser:{0}", CurrentIdUser));

                List<FilterOption> filters = new List<FilterOption>();

                if(!string.IsNullOrEmpty(filterName))
                    filters.Add(new FilterOption { Field = "UserName", Value = filterName, Sign = "%" });

                if (!string.IsNullOrEmpty(filterEmail))
                    filters.Add(new FilterOption { Field = "Email", Value = filterEmail, Sign = "%" });

                List<string> selectColumnsList = new List<string>
                {
                    "Id",
                    "UserName",
                    "Email",
                    "Disabled",
                    "LockoutEnabled",
                    "LastActivityDate",
                    "RoleName"
                };

                var pagedRecord = new PagedList
                {
                    Content = _userBL.GetAllView(filters, page, pageSize, sortBy, sortDirection, selectColumnsList),
                    TotalRecords = _userBL.CountGetAllView(filters, page, pageSize),
                    CurrentPage = page,
                    PageSize = pageSize
                };

                _logger.Debug(string.Format("finish GetAll ,idUser:{0}", CurrentIdUser));
                return Ok(pagedRecord);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return InternalServerError(ex);
            }
        }

        public IHttpActionResult GetUsers(int id)
        {
            try
            {
                _logger.Debug(string.Format("ini process - GetModel,idUser:{0}", CurrentIdUser));

                User user = _userBL.GetById(id);


                _logger.Debug(string.Format("finish GetModel - success,idUser:{0}, idUser model:{1}", CurrentIdUser, id));
                return Ok(user ?? new User());
            }
            catch (Exception ex)
            {
                LogError(ex);
                return InternalServerError(ex);
            }
        }

        public async Task<IHttpActionResult> PostUsers(UserModel model)
        {
            try
            {
                _logger.Debug(string.Format("ini process - Post,idUser:{0}", CurrentIdUser));

                if (!ModelState.IsValid)
                {
                    _logger.Debug(string.Format("ini Post - inValid,idUser:{0}", CurrentIdUser));
                    return BadRequest(ModelState);
                }

                User user = AutoMapper.Mapper.Map<User>(model);

                _logger.Debug(string.Format("ini create ,idUser:{0}", CurrentIdUser));

                IdentityResult addUserResult = _appUserManager.Create(user, model.Password);

                if (!addUserResult.Succeeded)
                {
                    _logger.Debug(string.Format("ini Post - GetErrorResult,idUser:{0}", CurrentIdUser));
                    return GetErrorResult(addUserResult);
                }

                _logger.Debug(string.Format("user Post info email:{0}, idUser:{1}", user.Email, user.Id));

                Role role = _roleBL.Get(model.IdRole);

                _logger.Debug(string.Format("user Post info role:{0}, idUser:{1}", role.Name, user.Id));

                _appUserManager.AddToRole(user.Id, role.Name);
                _appUserManager.AddClaim(user.Id, new Claim(ClaimTypes.Authentication, "local"));

                string generatedCode = await _appUserManager.GenerateEmailConfirmationTokenAsync(user.Id);

                string callbackUrl = string.Format("{0}/{1}/{2}", model.ConfirmURL, user.Id, System.Web.HttpUtility.UrlEncode(generatedCode));
     
                string bodyHtmlEmail = Helpers.CreateBodyEmail(user, callbackUrl);

                await _appUserManager.SendEmailAsync(user.Id, "Confirm your account", bodyHtmlEmail);
                _logger.Debug(string.Format("finish create ,idUser:{0},newIdUser{1}", CurrentIdUser, user.Id));

                return Ok(new JsonResponse { Success = true, Message = "User was Saved successfully", Data = user });
            }
            catch (Exception ex)
            {
                LogError(ex);
                return InternalServerError(ex);
            }
        }

        public IHttpActionResult PutUsers(int id, UserModel model)
        {
            try
            {
                if (id != model.Id)
                {
                    _logger.Debug(string.Format("ini Put - inValid,idUser:{0}", CurrentIdUser));
                    return BadRequest("Invalid ID");
                }

                _logger.Debug(string.Format("ini process - Put,idUser:{0}", CurrentIdUser));

                var user = _userBL.Get(model.Id);
                user.LockoutEnabled = model.LockoutEnabled;
                user.Disabled = model.Disabled;

                _logger.Debug(string.Format("ini Put ,idUser:{0},idUser update:{1}", CurrentIdUser, model.Id));

                _userBL.Update(user);
                var roles = _appUserManager.GetRoles(model.Id);
                _appUserManager.RemoveFromRoles(model.Id, roles.ToArray());

                Role role = _roleBL.Get(model.IdRole);

                _logger.Debug(string.Format("user update info role:{0}, idUser:{1}", role.Name, user.Id));
                _appUserManager.AddToRole(user.Id, role.Name);

                _logger.Debug(string.Format("finish update ,idUser:{0}", CurrentIdUser));


                _logger.Debug(string.Format("finish Put - success,idUser:{0}", CurrentIdUser));

                return Ok(new JsonResponse { Success = true, Message = "User was Saved successfully", Data = user });
            }
            catch (Exception ex)
            {
                LogError(ex);
                return InternalServerError(ex);
            }
        }
    }
}
