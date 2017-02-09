namespace Blog.Business.Entity
{
    public class File
    {
        public string IdStorage { get; set; }

        public string Name { get; set; }

        public int Type { get; set; }

        public string DownloadURL { get; set; }

        public string Url
        {
            get
            {
                return string.IsNullOrEmpty(ThumbUrl) ? string.Empty : ThumbUrl.Replace("=s220", "=s4000");
            }
        }

        public string ThumbUrl { get; set; }

        public string ParentFolderId { get; set; }
    }
}
