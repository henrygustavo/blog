namespace Blog.WebApiSite.Models
{
    using System.ComponentModel.DataAnnotations;

    public class UserModel : RegisterBindingModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int IdRole { get; set; }
         
        [Required]
        public bool LockoutEnabled { get; set; }

        [Required]
        public  bool Disabled { get; set; }
    }
}