namespace Blog.Business.Entity
{
    public class Folder
    {
        public string Id { get; set; }
        public string IdParent { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Link { get; set; }
        public string ParentFolderName { get; set; }
    }
}
