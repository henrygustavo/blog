namespace Blog.WebApiSite.Controllers
{
	using Business.Logic.Interfaces;
	using Common.Logging;
	using System;
	using System.Collections.Generic;
	using System.Web.Http;
	using Business.Entity;
	using Core;

    [Authorize(Roles = "admin")]
    [RoutePrefix("api/commons")]
    public class CommonsController :  BaseController
    {
        private readonly ILog _logger;
        private readonly ICommonBL _commonBL;
      
        public CommonsController(ILog logger, ICommonBL commonBL)
            : base(logger)
        {
            _logger = logger;
            _commonBL = commonBL;

        }

        [HttpGet]
        [Route("states")]
        public IHttpActionResult GetStates()
        {
            try
            {
                _logger.Debug(string.Format("ini process - GetStates,idUser: {0}", CurrentIdUser));

                List<Setting> settingList = _commonBL.GetByIdCategorySetting((int)EnumCategorySetting.States);

                var resultList = Helpers.ConvertToListItem(settingList, "Id", "Name");

                _logger.Debug(string.Format("finish GetStates - success,idUser: {0}", CurrentIdUser));
                return Ok(resultList);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("setting/{idSetting}")]   
        public IHttpActionResult GetSetting(int idSetting)
        {
            try
            {
                _logger.Debug(string.Format("ini process - GetSetting,idUser: {0}", CurrentIdUser));

                Setting setting = _commonBL.GetSetting(idSetting);

                _logger.Debug(string.Format("finish GetSetting - success,idUser: {0}", CurrentIdUser));
                return Ok(setting);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return InternalServerError(ex);
            }
        }
    }
}