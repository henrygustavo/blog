namespace Blog.WebApiSite.Core
{
    using Microsoft.Owin.Security.Facebook;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public class FacebookAuthProvider : FacebookAuthenticationProvider
    {
        public override Task Authenticated(FacebookAuthenticatedContext context)
        {
            context.Identity.AddClaim(new Claim("ExternalAccessToken", context.AccessToken));
            context.Identity.AddClaim(new Claim("Email", context.User.GetValue("email").ToString()));
            return Task.FromResult<object>(null);
        }
    }
}