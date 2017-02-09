namespace Blog.WebApiSite.Controllers
{
    using Common.Logging;
    using System;
    using System.Collections.Generic;
    using System.Web.Http;
	using Business.Entity;
    using Storage;
    using Business.Logic.Interfaces;
    using Core;

    [Authorize(Roles = "admin")]
    [RoutePrefix("api/files")]
    public class FilesController : BaseController
    {
        private readonly ILog _logger;
        private readonly IFileManager _iFileManager;
        private readonly ICommonBL _commonBL;

        public FilesController(ILog logger, IFileManager fileManager, ICommonBL commonBL)
            : base(logger)
        {
            _logger = logger;
            _iFileManager = fileManager;
            _commonBL = commonBL;

        }

        [HttpGet]
        public IHttpActionResult GetFiles(string id)
        {
            try
            {
                _logger.Debug(string.Format("ini process - GetFile,idUser: {0}, folder id:{1}", CurrentIdUser, id));

                File file = _iFileManager.GetFileById(id);

                _logger.Debug(string.Format("finish GetFile - success,idUser: {0},folder id:{1}", CurrentIdUser, id));
                return Ok(file);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("idFolder/{idFolder}")]
        public IHttpActionResult GetFilesByIdFolder(string idFolder)
        {
            try
            {
                _logger.Debug(string.Format("ini process - GetFiles,idUser: {0}, folder id:{1}", CurrentIdUser, idFolder));

                IList<File> files = _iFileManager.GetFilesByIdFolder(idFolder);

                _logger.Debug(string.Format("finish GetFiles - success,idUser: {0},folder id:{1}", CurrentIdUser, idFolder));
                return Ok(files);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("images/{type}")]
        public IHttpActionResult GetImages(string type)
        {
            try
            {
                IList<File> files = new List<File>();

                _logger.Debug(string.Format("ini process - GetImageProfile,idUser: {0}", CurrentIdUser));

                if (string.Compare(type,"profile",StringComparison.OrdinalIgnoreCase) == 0)
                {
                    Setting imageProfile = _commonBL.GetSetting((int) EnumSetting.IdProfileFolder);
                    files = _iFileManager.GetFilesByIdFolder(imageProfile.ParamValue);
                }
                _logger.Debug(string.Format("finish GetImageProfile - success,idUser: {0}", CurrentIdUser));

                return Ok(files);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return InternalServerError(ex);
            }
        }
    }
}
