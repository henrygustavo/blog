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
    [RoutePrefix("api/tags")]
	public class TagsController : BaseController
	{
		private readonly ILog _logger;
		private readonly ITagBL _tagBL;
		private readonly ICommonBL _commonBL;

		public TagsController(ILog logger,ITagBL tagBL, ICommonBL commonBL)
			: base(logger)
		{
			_logger = logger;
			_tagBL = tagBL;
			_commonBL = commonBL;

		}

		public IHttpActionResult GetTags(string filterName, int page = 1, int pageSize = 10, string sortBy = "id", string sortDirection = "asc")
		{
			try
			{
				_logger.Debug(string.Format("ini process - GetAll,idUser: {0}", CurrentIdUser));

				List<FilterOption> filters = new List<FilterOption>();

                if (!string.IsNullOrEmpty(filterName))
                    filters.Add(new FilterOption { Field = "Name", Value = filterName, Sign = "%" });


                List<string> selectColumnsList = new List<string>
				{
				"Id",   
				"Name",   
				"State"   
				};

				List<Tag> entities = _tagBL.GetAll(filters, page, pageSize, sortBy, sortDirection, selectColumnsList);

				List<Setting> settingList = _commonBL.GetByIdCategorySetting((int)EnumCategorySetting.States);

                var result = from entity in entities select new { 
				entity.Id,
				entity.Name,
				entity.State,
                StateName = settingList.SingleOrDefault(x => x.Id == entity.State).Name		
				};
				var pagedRecord = new PagedList
				{
					Content = result.ToList(),
					TotalRecords = _tagBL.CountGetAll(filters, page, pageSize),
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

        [Route("state/{state}")]
        [AllowAnonymous]
        public IHttpActionResult GetTags(string state)
        {
            List<Tag> tagList = new List<Tag>();

            try
            {
                switch (state.ToLower())
                {
                    case "active":

                        tagList = _tagBL.GetAll().Where(p => p.State == (int) EnumSetting.Active).ToList();
                        break;

                    case "inactive":
                        tagList = _tagBL.GetAll().Where(p => p.State == (int)EnumSetting.Inactive).ToList();
                        break;
                }
                
                return Ok(tagList);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return InternalServerError(ex);
            }
        }
        public IHttpActionResult GetTags(int id)
		{
			try
			{
				_logger.Debug(string.Format("ini process - GetModel,idUser: {0}", CurrentIdUser));

				Tag tag = _tagBL.Get(id);

				_logger.Debug(string.Format("finish GetModel - success,idUser: {0}, modelId: {1}", CurrentIdUser, id));
				return Ok(tag ?? new Tag());
			}
			catch (Exception ex)
			{
				LogError(ex);
				return InternalServerError(ex);
			}
		}

        public IHttpActionResult PostTags(TagModel model)
		{
			try
			{
				_logger.Debug(string.Format("ini process - Post,idUser:{0}", CurrentIdUser));

				if (!ModelState.IsValid)
				{
					_logger.Debug(string.Format("ini Post - inValid,idUser:{0}", CurrentIdUser));
					return BadRequest(ModelState);
				}

				 Tag tag = AutoMapper.Mapper.Map<Tag>(model);
					 tag.LastActivityIdUser = CurrentIdUser;
					 tag.CreationIdUser = CurrentIdUser;

				_tagBL.Insert(tag);
				_logger.Debug(string.Format("finish Post - success,idUser:{0}", CurrentIdUser));

				return Ok(new JsonResponse { Success = true, Message = "Tag was Saved successfully" , Data = tag});

			}
			catch (Exception ex)
			{
				LogError(ex);
				return InternalServerError(ex);
			}
		}
	 
		public IHttpActionResult PutTags(int id, TagModel model)
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

				 Tag tag = AutoMapper.Mapper.Map<Tag>(model);
				
					 tag.LastActivityIdUser = CurrentIdUser;
				
				_tagBL.Update(tag);

				_logger.Debug(string.Format("finish Put - success,idUser:{0}", CurrentIdUser));

				return Ok(new JsonResponse { Success = true, Message = "Tag was Saved successfully" , Data = tag});

			}
			catch (Exception ex)
			{
				LogError(ex);
				return InternalServerError(ex);
			}
		}
	}
}