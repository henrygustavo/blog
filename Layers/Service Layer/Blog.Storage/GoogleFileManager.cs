namespace Blog.Storage
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Business.Entity;
    using Business.Logic.Interfaces;
    using Google.Apis.Drive.v2;
    using log4net;

    public class GoogleFileManager : IFileManager
    {
        private readonly DriveService _service;
        private readonly IGoogleApiDataStoreBL _GoogleApiDataStoreBL;
        private readonly ILog _logger;

        public GoogleFileManager(IGoogleApiDataStoreBL GoogleApiDataStoreBL)
        {
            _GoogleApiDataStoreBL = GoogleApiDataStoreBL;
            _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

            _service = GoogleAuthentication.CreateServie(_GoogleApiDataStoreBL);
            _logger.Debug("finish autenticacion google drive");
        }

        public IList<File> GetFilesByFolderName(string folderName)
        {
            IList<File> files = new List<File>();

            // Listing files with search.  
            string query = string.Format("title = '{0}' and mimeType = 'application/vnd.google-apps.file'", folderName);
            IList<Google.Apis.Drive.v2.Data.File> driveFiles = GoogleDriveHelper.GetFiles(_service, query);

            foreach (Google.Apis.Drive.v2.Data.File driveFile in driveFiles)
            {

                files.Add(new File { Name = driveFile.Title, ThumbUrl = driveFile.ThumbnailLink });
            }
            return files;
        }

        public IList<Folder> GetFoldersByParentFolderName(string parentFolderName)
        {
            IList<Folder> folders = new List<Folder>();

            // Listing files with search.  

            IList<Google.Apis.Drive.v2.Data.File> driveFolders = GoogleDriveHelper.GetFolders(_service, parentFolderName);

            foreach (Google.Apis.Drive.v2.Data.File driveFolder in driveFolders)
            {
                if (driveFolder == null) continue;
                folders.Add(new Folder { Name = driveFolder.Title, Id = driveFolder.Id });
            }

            return folders;
        }

        public IList<Folder> GetFoldersByIdParentFolder(string idParentFolder)
        {
            IList<Folder> folders = new List<Folder>();

            // Listing files with search.  

            IList<Google.Apis.Drive.v2.Data.File> driveFolders = GoogleDriveHelper.GetFoldersByIdParentFolder(_service, idParentFolder);

            foreach (Google.Apis.Drive.v2.Data.File driveFolder in driveFolders)
            {
                if (driveFolder == null) continue;
                folders.Add(new Folder { Name = driveFolder.Title, Id = driveFolder.Id });
            }

            return folders;
        }

        public string CreateFolder(string title, string description, string idParentFolder)
        {
            Google.Apis.Drive.v2.Data.File folder = GoogleDriveHelper.CreateFolder(_service, title, description, idParentFolder);

            return folder.Id;
        }

        public string GetIdFolderByFolderName(string folderName)
        {
            return GoogleDriveHelper.GetIdFolderByFolderName(_service, folderName);
        }

        public bool ExistsFolderNameByIdParent(string folderName, string idParent)
        {
            return GoogleDriveHelper.ExistsFolderNameByIdParent(_service, folderName, idParent);
        }

        public IList<File> GetFilesByIdFolder(string idFolder)
        {
            int attempts = 0;

            IList<File> files = new List<File>();
            do
            {
                files = GetFilesAuxByIdFolder(idFolder);

                attempts++;

            } while (!files.Any() && attempts < 3);

            return files;
        }

        public List<Folder> GetFoldersBySearchName(string searchName)
        {
            _logger.Debug(string.Format("Ini GetFoldersBySearchName {0}", searchName));
            return GoogleDriveHelper.GetFoldersBySearchName(_service, searchName);
        }

        public Folder GetFolderByIdFolder(string idFolder)
        {
            Folder folder = new Folder();
            var googleFolder = GoogleDriveHelper.GetFileById(_service, idFolder);

            string idParent = string.Empty;
            string parentName = string.Empty;

            if (googleFolder != null)
            {
                if (googleFolder.Parents.Any())
                {
                    idParent = googleFolder.Parents[0].Id;
                }

                folder = new Folder { Id = googleFolder.Id, Name = googleFolder.Title, IdParent = idParent, Link = googleFolder.WebViewLink, ParentFolderName = parentName };
            }
            return folder;
        }

        public File GetFileById(string idFile)
        {
            Google.Apis.Drive.v2.Data.File file = GoogleDriveHelper.GetFileById(_service, idFile);

            if (file == null) return new File();

            File fileResult = new File
            {
                IdStorage = file.Id,
                Name = file.Title,
                ThumbUrl = file.ThumbnailLink,
                DownloadURL = file.DownloadUrl
            };

            return fileResult;

        }

        public IList<File> GetFilesByFolderIds(List<string> idFolderList)
        {
            if (!idFolderList.Any())
            {
                return new List<File>();
            }

            int attempts = 0;

            IList<File> files = new List<File>();
            do
            {
                files = GetFilesAuxByIdsFolders(idFolderList);

                attempts++;

            } while (!files.Any() && attempts < 3);

            return files;
        }

        public string UploadFile(string uploadFile, string parentIdFolder)
        {
            Google.Apis.Drive.v2.Data.File folder = GoogleDriveHelper.UploadFile(_service, uploadFile, parentIdFolder);
            return (folder == null) ? string.Empty : folder.Id;
        }

        private IList<File> GetFilesAuxByIdFolder(string idFolder)
        {
            List<File> fileList = new List<File>();

            var contents = GoogleDriveHelper.ListFolderContent(_service, idFolder);

            foreach (var image in contents)
            {
                if (image == null) continue;

                File file = new File
                {
                    IdStorage = image.Id,
                    Name = image.Title,
                    ThumbUrl = image.ThumbnailLink,
                    DownloadURL = image.DownloadUrl
                };

                fileList.Add(file);
            }

            return fileList;
        }

        private IList<File> GetFilesAuxByIdsFolders(List<string> idFolderList)
        {
            List<File> fileList = new List<File>();

            var contents = GoogleDriveHelper.ListFolderContent(_service, idFolderList);

            foreach (var image in contents)
            {
                if (image == null) continue;

                File file = new File
                {
                    IdStorage = image.Id,
                    Name = image.Title,
                    ThumbUrl = image.ThumbnailLink,
                    DownloadURL = image.DownloadUrl,
                    ParentFolderId = image.Parents[0].Id
                };

                fileList.Add(file);
            }

            return fileList;
        }
    }
}
