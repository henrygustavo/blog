namespace Blog.Storage
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Business.Entity;
    using Google.Apis.Drive.v2;
    using Google.Apis.Drive.v2.Data;
    using log4net;
    using Microsoft.Win32;
    using File = Google.Apis.Drive.v2.Data.File;

    public static class GoogleDriveHelper
    {
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static List<File> ListRootFileFolders(DriveService service)
        {
            List<File> result = new List<File>();
            FilesResource.ListRequest request = service.Files.List();
            do
            {
                try
                {
                    FileList files = request.Execute();

                    result.AddRange(files.Items);
                    request.PageToken = files.NextPageToken;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            } while (!string.IsNullOrEmpty(request.PageToken));
            return result;
        }

        /// <summary>
        /// Download a file
        /// Documentation: https://developers.google.com/drive/v2/reference/files/get
        /// </summary>
        /// <param name="service">a Valid authenticated DriveService</param>
        /// <param name="fileResource">File resource of the file to download</param>
        /// <param name="saveTo">location of where to save the file including the file name to save it as.</param>
        /// <returns></returns>
        public static bool DownloadFile(DriveService service, File fileResource, string saveTo)
        {

            if (!string.IsNullOrEmpty(fileResource.DownloadUrl))
            {
                try
                {
                    var x = service.HttpClient.GetByteArrayAsync(fileResource.DownloadUrl);
                    byte[] arrBytes = x.Result;
                    System.IO.File.WriteAllBytes(saveTo, arrBytes);
                    return true;
                }
                catch (Exception ex)
                {

                    throw new Exception(ex.Message);
                }
            }
            // The file doesn't have any content stored on Drive.
            return false;
        }

        private static string GetMimeType(string fileName)
        {
            string mimeType = "application/unknown";
            string ext = Path.GetExtension(fileName).ToLower();
            RegistryKey regKey = Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }

        /// <summary>
        /// Uploads a file
        /// Documentation: https://developers.google.com/drive/v2/reference/files/insert
        /// </summary>
        /// <param name="service">a Valid authenticated DriveService</param>
        /// <param name="uploadFile">path to the file to upload</param>
        /// <param name="parentIdFolder">Collection of parent folders which contain this file. 
        ///                       Setting this field will put the file in all of the provided folders. root folder.</param>
        /// <returns>If upload succeeded returns the File resource of the uploaded file 
        ///          If the upload fails returns null</returns>
        public static File UploadFile(DriveService service, string uploadFile, string parentIdFolder)
        {
            if (System.IO.File.Exists(uploadFile))
            {
                File body = new File
                {
                    Title = Path.GetFileName(uploadFile),
                    Description = "File uploaded",
                    MimeType = GetMimeType(uploadFile),
                    Parents = new List<ParentReference> {new ParentReference {Id = parentIdFolder } }
                };

                // File's content.
                byte[] byteArray = System.IO.File.ReadAllBytes(uploadFile);
                MemoryStream stream = new MemoryStream(byteArray);
                try
                {
                    FilesResource.InsertMediaUpload request = service.Files.Insert(body, stream, GetMimeType(uploadFile));
                    request.Upload();
                    return request.ResponseBody;
                }
                catch (Exception e)
                {
                    Console.WriteLine("An error occurred: " + e.Message);
                    return null;
                }
            }
            Console.WriteLine("File does not exist: " + uploadFile);
            return null;
        }

        /// <summary>
        /// Updates a file
        /// Documentation: https://developers.google.com/drive/v2/reference/files/update
        /// </summary>
        /// <param name="service">a Valid authenticated DriveService</param>
        /// <param name="uploadFile">path to the file to upload</param>
        /// <param name="parentIdFolder">Collection of parent folders which contain this file. 
        ///                       Setting this field will put the file in all of the provided folders. root folder.</param>
        /// <param name="idFile">the resource id for the file we would like to update</param>                      
        /// <returns>If upload succeeded returns the File resource of the uploaded file 
        ///          If the upload fails returns null</returns>
        public static File UpdateFile(DriveService service, string uploadFile, string parentIdFolder, string idFile)
        {
            if (System.IO.File.Exists(uploadFile))
            {
                File body = new File
                {
                    Title = Path.GetFileName(uploadFile),
                    Description = "File updated Drive",
                    MimeType = GetMimeType(uploadFile),
                    Parents = new List<ParentReference> {new ParentReference {Id = parentIdFolder } }
                };

                // File's content.
                byte[] byteArray = System.IO.File.ReadAllBytes(uploadFile);
                MemoryStream stream = new MemoryStream(byteArray);
                try
                {
                    FilesResource.UpdateMediaUpload request = service.Files.Update(body, idFile, stream, GetMimeType(uploadFile));
                    request.Upload();
                    return request.ResponseBody;
                }
                catch (Exception ex)
                {

                    throw new Exception(ex.Message);
                }
            }
            return null;
        }


        /// <summary>
        /// Create a new Directory.
        /// Documentation: https://developers.google.com/drive/v2/reference/files/insert
        /// </summary>
        /// <param name="service">a Valid authenticated DriveService</param>
        /// <param name="title">The title of the file. Used to identify file or folder name.</param>
        /// <param name="description">A short description of the file.</param>
        /// <param name="idParentFolder">Collection of parent folders which contain this file. 
        ///                       Setting this field will put the file in all of the provided folders. root folder.</param>
        /// <returns></returns>
        public static File CreateFolder(DriveService service, string title, string description, string idParentFolder)
        {

            File newDirectory = null;

            // Create metaData for a new Directory
            File body = new File
            {
                Title = title,
                Description = description,
                MimeType = "application/vnd.google-apps.folder",
                Parents = new List<ParentReference> {new ParentReference {Id = idParentFolder } }
            };
            try
            {
                FilesResource.InsertRequest request = service.Files.Insert(body);
                newDirectory = request.Execute();

                Permission permission = new Permission
                {
                    Value = "",
                    Type = "anyone",
                    Role = "reader"
                };

                service.Permissions.Insert(permission, newDirectory.Id).Execute();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return newDirectory;
        }

        /// <summary>
        /// List all of the files and directories for the current user.  
        /// 
        /// Documentation: https://developers.google.com/drive/v2/reference/files/list
        /// Documentation Search: https://developers.google.com/drive/web/search-parameters
        /// </summary>
        /// <param name="service">a Valid authenticated DriveService</param>        
        /// <param name="search">if Search is null will return all files</param>        
        /// <returns></returns>
        public static IList<File> GetFiles(DriveService service, string search)
        {

            IList<File> files = new List<File>();

            try
            {
                //List all of the files and directories for the current user.  
                // Documentation: https://developers.google.com/drive/v2/reference/files/list
                FilesResource.ListRequest list = service.Files.List();

                if (search != null)
                {
                    list.Q = search;
                }
                FileList filesFeed = list.Execute();

                //// Loop through until we arrive at an empty page
                while (filesFeed.Items != null)
                {
                    // Adding each item  to the list.
                    foreach (File item in filesFeed.Items)
                    {
                        files.Add(item);
                    }

                    // We will know we are on the last page when the next page token is
                    // null.
                    // If this is the case, break.
                    if (filesFeed.NextPageToken == null)
                    {
                        break;
                    }

                    // Prepare the next page of results
                    list.PageToken = filesFeed.NextPageToken;

                    // Execute and process the next page request
                    filesFeed = list.Execute();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return files;
        }

        public static IList<File> GetFolders(DriveService service, string parentFolderName)
        {

            string idParentFolder = GetIdFolderByFolderName(service, parentFolderName);

            return GetFoldersByIdParentFolder(service, idParentFolder);
        }

        public static IList<File> GetFoldersByIdParentFolder(DriveService service, string idParentFolder)
        {
            IList<File> folders = new List<File>();

            try
            {
                ChildrenResource.ListRequest list = service.Children.List(idParentFolder);

                list.Q = "mimeType = 'application/vnd.google-apps.folder' and trashed=false";

                do
                {
                    try
                    {
                        ChildList children = list.Execute();

                        foreach (ChildReference child in children.Items)
                        {
                            folders.Add(GetFileById(service, child.Id));
                        }
                        list.PageToken = children.NextPageToken;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                } while (!string.IsNullOrEmpty(list.PageToken));

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return folders;
        }

        public static string GetIdFolderByFolderName(DriveService service, string folderName)
        {
            FilesResource.ListRequest request = service.Files.List();
            request.Q = string.Format("title = '{0}'and trashed=false and mimeType = 'application/vnd.google-apps.folder'", folderName);
            FileList files = request.Execute();
            return (files.Items != null && files.Items.Count > 0) ? files.Items[0].Id : string.Empty;
        }

        public static List<Folder> GetFoldersBySearchName(DriveService service, string searchName)
        {
            _logger.Debug(string.Format("Ini GetFoldersBySearchName {0}", searchName));

            List<Folder> listFolders = new List<Folder>();

            //List all of the files and directories for the current user.  
            // Documentation: https://developers.google.com/drive/v2/reference/files/list
            FilesResource.ListRequest list = service.Files.List();

            _logger.Debug("GetFoldersBySearchName Ini query");

            list.Q = string.Format("title contains '{0}' and mimeType = 'application/vnd.google-apps.folder' and trashed=false", searchName); ;

            _logger.Debug("GetFoldersBySearchName Execute");

            FileList filesFeed = list.Execute();
            List<string> idParentList = new List<string>();

            _logger.Debug("GetFoldersBySearchName Loop");
            //// Loop through until we arrive at an empty page
            while (filesFeed.Items != null)
            {
                // Adding each item  to the list.
                foreach (File item in filesFeed.Items)
                {
                    string idParent = string.Empty;

                    if (item.Parents.Any())
                    {
                        idParent = item.Parents[0].Id;

                        if (!string.IsNullOrEmpty(idParent))
                            idParentList.Add(idParent);
                    }

                    listFolders.Add(new Folder { Id = item.Id, Name = item.Title, Link = item.WebViewLink, IdParent = idParent });
                }

                // We will know we are on the last page when the next page token is
                // null.
                // If this is the case, break.
                if (filesFeed.NextPageToken == null)
                {
                    break;
                }

                // Prepare the next page of results
                list.PageToken = filesFeed.NextPageToken;

                // Execute and process the next page request
                filesFeed = list.Execute();
            }

            _logger.Debug("GetFoldersBySearchName finish Loop");

            List<File> parentFolderList = new List<File>();

            foreach (string idParent in idParentList.Distinct())
            {
                var folder = GetFileById(service, idParent);
                if (folder != null)
                    parentFolderList.Add(folder);
            }

            if (parentFolderList.Any())
            {
                foreach (var folder in listFolders)
                {
                    var parentFolder = parentFolderList.SingleOrDefault(p => p.Id == folder.IdParent);
                    folder.ParentFolderName = parentFolder != null ? parentFolder.Title : string.Empty;
                }
            }

            return listFolders;
        }

        public static File GetFileById(DriveService service, string idFile)
        {
            File file = service.Files.Get(idFile).Execute();
            if (file.ExplicitlyTrashed == false || file.ExplicitlyTrashed == null)
                return file;
            return null;
        }

        public static bool ExistsFolderNameByIdParent(DriveService service, string folderName, string idParent)
        {
            FilesResource.ListRequest request = service.Files.List();
            request.Q = string.Format("title = '{0}' and mimeType = 'application/vnd.google-apps.folder' and trashed=false and '{1}' in parents", folderName, idParent);
            FileList files = request.Execute();
            return files.Items != null && files.Items.Count != 0;
        }

        public static List<File> ListFolderContent(DriveService service, string idFolder)
        {
            ChildrenResource.ListRequest request = service.Children.List(idFolder);
            List<File> files = new List<File>();

            request.Q = string.Format("'{0}' in parents", idFolder);

            do
            {
                try
                {
                    ChildList children = request.Execute();

                    foreach (ChildReference child in children.Items)
                    {
                        files.Add(GetFileById(service, child.Id));
                    }

                    request.PageToken = children.NextPageToken;

                }
                catch
                {
                    request.PageToken = null;
                }
            } while (!string.IsNullOrEmpty(request.PageToken));
            return files;
        }

        public static List<File> ListFolderContent(DriveService service, List<string> idFolderList)
        {
            string query = "mimeType contains 'image/' and trashed=false and (";

            foreach (string idFolder in idFolderList)
            {
                query += string.Format(" or '{0}' in parents", idFolder);
            }

            List<File> files = new List<File>();

            try
            {
                //List all of the files and directories for the current user.  
                // Documentation: https://developers.google.com/drive/v2/reference/files/list
                FilesResource.ListRequest list = service.Files.List();

                list.Q = string.Format("{0})", query.Replace("and ( or", "and ("));

                FileList filesFeed = list.Execute();

                //// Loop through until we arrive at an empty page
                while (filesFeed.Items != null)
                {
                    // Adding each item  to the list.
                    foreach (File item in filesFeed.Items)
                    {
                        files.Add(item);
                    }

                    // We will know we are on the last page when the next page token is
                    // null.
                    // If this is the case, break.
                    if (filesFeed.NextPageToken == null)
                    {
                        break;
                    }

                    // Prepare the next page of results
                    list.PageToken = filesFeed.NextPageToken;

                    // Execute and process the next page request
                    filesFeed = list.Execute();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return files;
        }
    }
}
