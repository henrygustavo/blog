namespace Blog.WebApiSite.Models
{
    using System.ComponentModel.DataAnnotations;

    public class RoleModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}