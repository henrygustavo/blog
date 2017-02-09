using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.Google;

namespace Blog.WebApiSite
{
	using Core;
	using Microsoft.Owin;
	using Microsoft.Owin.Security;
	using Microsoft.Owin.Security.DataHandler.Encoder;
	using Microsoft.Owin.Security.DataProtection;
	using Microsoft.Owin.Security.Jwt;
	using Microsoft.Owin.Security.OAuth;
	using Newtonsoft.Json.Serialization;
	using Owin;
	using System;
	using System.Configuration;
	using System.Linq;
	using System.Net.Http.Formatting;
	using System.Web.Http;
	using System.Web.Mvc;
	public partial class Startup
	{
		internal static IDataProtectionProvider DataProtectionProvider { get; private set; }
		public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; private set; }
		public static GoogleOAuth2AuthenticationOptions GoogleAuthOptions { get; private set; }
		public static OAuthAuthorizationServerOptions OAuthServerOptions { get; private set; }
		public static FacebookAuthenticationOptions FacebookAuthOptions { get; private set; }
		private void ConfigureOAuthTokenGeneration(IAppBuilder app)
		{
			//use a cookie to temporarily store information about a user logging in with a third party login provider
			app.UseExternalSignInCookie(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ExternalCookie);
			OAuthBearerOptions = new OAuthBearerAuthenticationOptions();

			DataProtectionProvider = app.GetDataProtectionProvider();

			// Configure the db context, user manager and signin manager to use a single instance per request
			app.CreatePerOwinContext(() => DependencyResolver.Current.GetService<ApplicationUserManager>());
			app.CreatePerOwinContext(() => DependencyResolver.Current.GetService<ApplicationSignInManager>());

			OAuthServerOptions = new OAuthAuthorizationServerOptions()
			{
				//For Dev enviroment only (on production should be AllowInsecureHttp = false)
				AllowInsecureHttp = true,
				TokenEndpointPath = new PathString("/api/oauth/token"),
				AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
				Provider = new CustomOAuthProvider(),
				AccessTokenFormat = new CustomJwtFormat("http://localhost:3030/")
			};

			// Token Generation
			app.UseOAuthAuthorizationServer(OAuthServerOptions);
			app.UseOAuthBearerAuthentication(OAuthBearerOptions);

			//Configure Google External Login
			GoogleAuthOptions = new GoogleOAuth2AuthenticationOptions()
			{
				ClientId = ConfigurationManager.AppSettings["googleClientId"],
				ClientSecret = ConfigurationManager.AppSettings["googleClientSecret"],
				Provider = new GoogleAuthProvider()
			};
			GoogleAuthOptions.Scope.Add("https://www.googleapis.com/auth/plus.login");
			GoogleAuthOptions.Scope.Add("email");
			GoogleAuthOptions.Scope.Add("profile");
			app.UseGoogleAuthentication(GoogleAuthOptions);

						//Configure Facebook External Login
			FacebookAuthOptions = new FacebookAuthenticationOptions()
			{
				AppId = ConfigurationManager.AppSettings["facebookClientId"],
				AppSecret = ConfigurationManager.AppSettings["facebookClientSecret"],
				Provider = new FacebookAuthProvider()
			};

			FacebookAuthOptions.Scope.Add("email");
			app.UseFacebookAuthentication(FacebookAuthOptions);  
		}

		private void ConfigureOAuthTokenConsumption(IAppBuilder app)
		{
			const string issuer = "http://localhost:3030/";
			string audienceId = ConfigurationManager.AppSettings["as:AudienceId"];
			byte[] audienceSecret = TextEncodings.Base64Url.Decode(ConfigurationManager.AppSettings["as:AudienceSecret"]);

			// Api controllers with an [Authorize] attribute will be validated with JWT
			app.UseJwtBearerAuthentication(
				new JwtBearerAuthenticationOptions
				{
					AuthenticationMode = AuthenticationMode.Active,
					AllowedAudiences = new[] { audienceId },
					IssuerSecurityTokenProviders = new IIssuerSecurityTokenProvider[]
					{
						new SymmetricKeyIssuerSecurityTokenProvider(issuer, audienceSecret)
					}
				});
		}

		private void ConfigureWebApi(HttpConfiguration config)
		{
			config.MapHttpAttributeRoutes();

			var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
			jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
		}
	}
}
