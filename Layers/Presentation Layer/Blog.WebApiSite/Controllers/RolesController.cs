namespace Blog.WebApiSite.Controllers
{
	using Business.Logic.Interfaces;
	using Models;
	using Common.Logging;
	using System;
	using System.Collections.Generic;
	using System.Web.Http;
	using Business.Entity;
	using System.Linq;
	using Core;

	[Authorize(Roles = "admin")]
    [RoutePrefix("api/roles")]
    public class RolesController : BaseController
    {
        private readonly ILog _logger;
        private readonly IRoleBL _roleBL;
        public RolesController(ILog logger, IRoleBL roleBL)
            : base(logger)
        {
            _logger = logger;
            _roleBL = roleBL;
        }

        [HttpGet]
        [Route("state/{state}")]
        public IHttpActionResult GetRoles(string state)
        {
            try
            {
                List<Role> roleList = new List<Role>();

                if (string.Compare(state,"active",StringComparison.OrdinalIgnoreCase) == 0)
                {
                    roleList = _roleBL.GetAll().ToList();
                    _logger.Debug(string.Format("ini process - RolesList,idUser:{0}", CurrentIdUser));
                   
                }

                return Ok(Helpers.ConvertToListItem(roleList, "Id", "Name"));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return InternalServerError(ex);
            }
        }
      
        public IHttpActionResult GetRoles(string filterName, int page = 1, int pageSize = 10, string sortBy = "Id", string sortDirection = "asc")
        {
            try
            {
                _logger.Debug(string.Format("ini process - GetAll,idUser:{0}", CurrentIdUser));

                List<FilterOption> filters = new List<FilterOption>();

                if (!string.IsNullOrEmpty(filterName))
                    filters.Add(new FilterOption { Field = "Name", Value = filterName, Sign = "%" });

                List<string> selectColumnsList = new List<string>
                {
                    "Id",
                    "Name"
                };

                var pagedRecord = new PagedList
                {
                    Content = _roleBL.GetAll(filters, page, pageSize, sortBy, sortDirection, selectColumnsList),
                    TotalRecords = _roleBL.CountGetAll(filters, page, pageSize),
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
      
        public IHttpActionResult GetRoles(int id)
        {
            try
            {
                _logger.Debug(string.Format("ini process - GetModel,idUser:{0}", CurrentIdUser));

                Role role = _roleBL.Get(id);

                _logger.Debug(string.Format("finish GetModel - success,idUser:{0}, idRole{1}", CurrentIdUser, id));
                return Ok(role ?? new Role());
            }
            catch (Exception ex)
            {
                LogError(ex);
                return InternalServerError(ex);
            }
        }
    
        public IHttpActionResult PostRoles(RoleModel model)
        {
            try
            {
                _logger.Debug(string.Format("ini process - Post,idUser:{0}", CurrentIdUser));

                if (!ModelState.IsValid)
                {
                    _logger.Debug(string.Format("ini Post - inValid,idUser:{0}", CurrentIdUser));
                    return BadRequest(ModelState);
                }

                Role role = AutoMapper.Mapper.Map<Role>(model);
                _roleBL.Insert(role);
                _logger.Debug(string.Format("finish Post - success,idUser:{0}", CurrentIdUser));

                return Ok(new JsonResponse { Success = true, Message = "Role was Saved successfully", Data = role });

            }
            catch (Exception ex)
            {
                LogError(ex);
                return InternalServerError(ex);
            }
        }
     
        public IHttpActionResult PutRoles(int id, RoleModel model)
        {
            try
            {
                _logger.Debug(string.Format("ini process - Put,idUser:{0}", CurrentIdUser));

                if (id != model.Id)
                {
                    _logger.Debug(string.Format("ini Put - inValid,idUser:{0}", CurrentIdUser));
                     return BadRequest("Invalid ID");
                }

                if (!ModelState.IsValid)
                {
                    _logger.Debug(string.Format("ini Put - inValid,idUser:{0}", CurrentIdUser));
                    return BadRequest(ModelState);
                }

                Role role = AutoMapper.Mapper.Map<Role>(model);
                _roleBL.Update(role);
                _logger.Debug(string.Format("finish Put - success,idUser:{0}", CurrentIdUser));

                return Ok(new JsonResponse { Success = true, Message = "Role was Saved successfully", Data = role });

            }
            catch (Exception ex)
            {
                LogError(ex);
                return InternalServerError(ex);
            }
        }
    }
}