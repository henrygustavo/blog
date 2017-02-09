namespace Blog.Business.Entity
{
    using Microsoft.AspNet.Identity;
    public class Role : Entity,IRole<int>
    {
        //public virtual string Name { get; set; }

        /// <summary>
        /// Default constructor for Role 
        /// </summary>
        public Role()
        {
           
        }
        /// <summary>
        /// Constructor that takes names as argument 
        /// </summary>
        /// <param name="name"></param>
        public Role(string name) : this()
        {
            Name = name;
        }

        public Role(string name, int id)
        {
            Name = name;
            Id = id;
        }
        /// <summary>
        /// Role name
        /// </summary>
        public virtual string Name { get; set; }
    }
}