namespace Blog.WebApiSite.Core
{
    using Business.Entity;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin.Security;

    public class ApplicationSignInManager : SignInManager<User, int>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager) :
            base(userManager, authenticationManager) { }
    }
}