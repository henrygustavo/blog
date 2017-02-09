namespace Blog.WebApiSite.Models
{
    using System;
    using System.Collections;
    public class PagedList
    {
        public IList Content { get; set; }

        public Int32 CurrentPage { get; set; }
        public Int32 PageSize { get; set; }
        public int TotalRecords { get; set; }

        public int TotalPages
        {
            get { return (int)Math.Ceiling((decimal)TotalRecords / PageSize); }
        }
    }
}