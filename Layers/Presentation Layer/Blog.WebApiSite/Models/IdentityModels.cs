namespace Blog.WebApiSite.Models
{
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.ComponentModel.DataAnnotations;
    using System;

    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required]
        public byte Level { get; set; }

        //public string Token { get; set; }
        [Required]
        public DateTime JoinDate { get; set; }

        public async Task<ClaimsIdentity> GenerateIdUserentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            var idUserentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here

            return idUserentity;
        }
    }
}