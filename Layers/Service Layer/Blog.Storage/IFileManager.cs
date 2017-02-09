namespace Blog.Storage
{
    using System.Collections.Generic;
    using Business.Entity;

    public interface IFileManager
    {
        IList<File> GetFilesByFolderName(string folderName);
        IList<Folder> GetFoldersByParentFolderName(string parentFolderName);
        IList<Folder> GetFoldersByIdParentFolder(string idParentFolder);
        string CreateFolder(string title, string description, string idParentFolder);
        string GetIdFolderByFolderName(string folderName);
        bool ExistsFolderNameByIdParent(string folderName, string idParent);
        IList<File> GetFilesByIdFolder(string idFolder);
        List<Folder> GetFoldersBySearchName(string searchName);
        Folder GetFolderByIdFolder(string idFolder);
        File GetFileById(string idFile);
        IList<File> GetFilesByFolderIds(List<string> idFolderList);
        string UploadFile(string uploadFile, string parentIdFolder);

    }
}
