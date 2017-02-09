namespace Blog.Business.Entity
{
    using System;

    public class BlogEntryView
    {
        public virtual int Id { get; set; }

        public virtual string Header { get; set; }

        public virtual string HeaderUrl { get; set; }

        public virtual string Author { get; set; }

        public virtual int State { get; set; }

        public virtual string  StateName { get; set; }

        public virtual DateTime CreationDate { get; set; }

        public virtual int TotalComments { get; set; }

        public virtual string Tags { get; set; }

        public virtual string ShortContent { get; set; }

        public virtual string Content { get; set; }

    }
}
