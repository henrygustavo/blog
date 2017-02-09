namespace Blog.Business.Entity
{
    using System;
    /// <summary>
    /// Base class for all Entity types.
    /// </summary>
    public class Entity : IEntity
    {
        /// <summary>
        /// Unique identifier for this entity.
        /// </summary>
        public virtual int Id { get; set; }
        public virtual int CreationIdUser { get; set; }
        public virtual int LastActivityIdUser { get; set; }
        public virtual DateTime CreationDate { get; set; }
        public virtual DateTime LastActivityDate { get; set; }
        public virtual bool IsLockedOut { get; set; }
    }
}