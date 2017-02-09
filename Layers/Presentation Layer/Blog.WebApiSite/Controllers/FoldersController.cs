namespace Blog.WebApiSite.Controllers
{
    using Common.Logging;
    using System;
    using System.Collections.Generic;
    using System.Web.Http;
    using Business.Entity;
    using Storage;
    using Models;
    using Business.Logic.Interfaces;
    using Core;

    [Authorize(Roles = "admin")]
    [RoutePrefix("api/folders")]
    public class FoldersController : BaseController
    {
        private readonly ILog _logger;
        private readonly IFileManager _iFileManager;
        private readonly ICommonBL _commonBL;

        public FoldersController(ILog logger, IFileManager fileManager, ICommonBL commonBL)
            : base(logger)
        {
            _logger = logger;
            _iFileManager = fileManager;
            _commonBL = commonBL;

        }

        [HttpGet]
        public IHttpActionResult GetFolders(string searchName = "", string idFolder = "")
        {
            try
            {
                _logger.Debug(string.Format("ini process - SearchFolders,idUser: {0}, searchName:{1},idFolder:{2}", CurrentIdUser, searchName, idFolder));

                List<Folder> folders = new List<Folder>();

                if (!string.IsNullOrEmpty(idFolder))
                {
                    folders.Add(_iFileManager.GetFolderByIdFolder(idFolder));
                }
                else
                {
                    folders = _iFileManager.GetFoldersBySearchName(searchName);
                }
                _logger.Debug(string.Format("finish SearchFolders - success,idUser: {0},searchName:{1},idFolder:{2}", CurrentIdUser, searchName, idFolder));

                var pagedRecord = new PagedList
                {
                    Content = folders,
                    TotalRecords = folders.Count,
                    PageSize = folders.Count > 0 ? folders.Count : 1,
                    CurrentPage = 1
                };
                return Ok(pagedRecord);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("content/{content}")]
        public IHttpActionResult GetFolders(string content)
        {
            try
            {
                IList<Folder> folders = new List<Folder>();

                _logger.Debug(string.Format("ini process - GetImageFolders,idUser: {0}", CurrentIdUser));

                switch (content.ToLower())
                {
                    case "files":
                        Setting fileSettingetting = _commonBL.GetSetting((int)EnumSetting.IdFilesFolder);
                        folders = _iFileManager.GetFoldersByIdParentFolder(fileSettingetting.ParamValue);
                        break;

                    case "images":
                        Setting imageSetting = _commonBL.GetSetting((int)EnumSetting.IdImagesFolder);
                        folders = _iFileManager.GetFoldersByIdParentFolder(imageSetting.ParamValue);
                        break;
                }
                
                _logger.Debug(string.Format("finish GetImageFolders - success,idUser: {0}", CurrentIdUser));

                return Ok(folders);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return InternalServerError(ex);
            }
        } 
    }
}