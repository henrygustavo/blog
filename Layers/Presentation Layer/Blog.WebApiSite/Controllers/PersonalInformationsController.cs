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
    [RoutePrefix("api/personalInformations")]
	public class PersonalInformationsController : BaseController
	{
		private readonly ILog _logger;
		private readonly IPersonalInformationBL _personalinformationBL;
		private readonly ICommonBL _commonBL;

		public PersonalInformationsController(ILog logger,IPersonalInformationBL personalinformationBL, ICommonBL commonBL)
			: base(logger)
		{
			_logger = logger;
			_personalinformationBL = personalinformationBL;
			_commonBL = commonBL;

		}

		public IHttpActionResult GetPersonalInformations(int page = 1, int pageSize = 10, string sortBy = "id", string sortDirection = "asc")
		{
			try
			{
				_logger.Debug(string.Format("ini process - GetAll,idUser: {0}", CurrentIdUser));

				List<FilterOption> filters = new List<FilterOption>();

				List<string> selectColumnsList = new List<string>
				{
				"Id",   
				"FirstName",   
				"LastName",   
				"SiteName",   
				"Email",   
				"Country",   
				"PhoneNumber",   
				"IdPhoto",   
				"Description",   
				"FaceBook",   
				"Twitter",   
				"GooglePlus"   
				};

				List<PersonalInformation> entities = _personalinformationBL.GetAll(filters, page, pageSize, sortBy, sortDirection, selectColumnsList);
				var pagedRecord = new PagedList
				{
					Content = entities.ToList(),
					TotalRecords = _personalinformationBL.CountGetAll(filters, page, pageSize),
					CurrentPage = page,
					PageSize = pageSize
				};

				_logger.Debug(string.Format("finish GetAll , idUser: {0}", CurrentIdUser));
				return Ok(pagedRecord);
			}
			catch (Exception ex)
			{
				LogError(ex);
				return InternalServerError(ex);
			}
		}

        [AllowAnonymous]
        public IHttpActionResult GetPersonalInformations(int id)
		{
			try
			{
				_logger.Debug(string.Format("ini process - GetModel,idUser: {0}", CurrentIdUser));

				PersonalInformation personalinformation = _personalinformationBL.Get(id);

				_logger.Debug(string.Format("finish GetModel - success,idUser: {0}, modelId: {1}", CurrentIdUser, id));
				return Ok(personalinformation ?? new PersonalInformation());
			}
			catch (Exception ex)
			{
				LogError(ex);
				return InternalServerError(ex);
			}
		}

		public IHttpActionResult PostPersonalInformations(PersonalInformationModel model)
		{
			try
			{
				_logger.Debug(string.Format("ini process - Post,idUser:{0}", CurrentIdUser));

				if (!ModelState.IsValid)
				{
					_logger.Debug(string.Format("ini Post - inValid,idUser:{0}", CurrentIdUser));
					return BadRequest(ModelState);
				}

				 PersonalInformation personalinformation = AutoMapper.Mapper.Map<PersonalInformation>(model);
					 personalinformation.LastActivityIdUser = CurrentIdUser;
					 personalinformation.CreationIdUser = CurrentIdUser;

				_personalinformationBL.Insert(personalinformation);
				_logger.Debug(string.Format("finish Post - success,idUser:{0}", CurrentIdUser));

				return Ok(new JsonResponse { Success = true, Message = "PersonalInformation was Saved successfully" , Data = personalinformation});

			}
			catch (Exception ex)
			{
				LogError(ex);
				return InternalServerError(ex);
			}
		}
	 
		public IHttpActionResult PutPersonalInformations(int id, PersonalInformationModel model)
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

				_logger.Debug(string.Format("ini update ,idUser: {0}, modelId update: {1}", CurrentIdUser, id));

				 PersonalInformation personalinformation = AutoMapper.Mapper.Map<PersonalInformation>(model);
				
					 personalinformation.LastActivityIdUser = CurrentIdUser;
				
				_personalinformationBL.Update(personalinformation);

				_logger.Debug(string.Format("finish Put - success,idUser:{0}", CurrentIdUser));

				return Ok(new JsonResponse { Success = true, Message = "PersonalInformation was Saved successfully" , Data = personalinformation});

			}
			catch (Exception ex)
			{
				LogError(ex);
				return InternalServerError(ex);
			}
		}
	}
}