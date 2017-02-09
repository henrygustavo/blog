namespace Blog.Business.Entity
{
    /// <summary>
    /// Defines interface for base entity type.
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Primary key of the entity.
        /// </summary>
        int Id { get; set; }
    }
}