namespace Blog.WebApiSite.Controllers
{
	using System;
	using System.Configuration;
	using System.Net.Http;
	using System.Security.Claims;
	using System.Threading.Tasks;
	using System.Web.Http;
	using Business.Entity;
	using Core;
	using Models;
	using Common.Logging;
	using Microsoft.AspNet.Identity;
	using Microsoft.AspNet.Identity.Owin;
	using Microsoft.Owin.Security;
	using Microsoft.Owin.Security.Cookies;
	using System.Web;
	using System.IO;

	[System.Web.Http.RoutePrefix("api/Account")]
	public class AccountController : BaseController
	{
		private readonly ApplicationUserManager _appUserManager;
		private readonly ApplicationSignInManager _appSignInManager;
		private readonly ILog _logger;
		private IAuthenticationManager Authentication
		{
			get { return Request.GetOwinContext().Authentication; }
		}

		public AccountController(ApplicationUserManager appUserManager, ApplicationSignInManager appSignInrManager, ILog logger)
			: base(logger)
		{
			_appUserManager = appUserManager;
			_appSignInManager = appSignInrManager;
			_logger = logger;
		}

		// POST api/Account/Logout
		[System.Web.Http.Route("Logout")]
		public IHttpActionResult Logout()
		{
			_logger.Debug("ini Logout process");
			Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
			_logger.Debug("Logout ok");
			return Ok();
		}

		// POST api/Account/ChangePassword
		[System.Web.Http.Route("ChangePassword")]
		public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
		{
			var idUser = User.Identity.GetUserId();

			_logger.Debug(string.Format("ini ChangePassword process, idUser:{0}", idUser));

			if (!ModelState.IsValid)
			{
				_logger.Debug(string.Format("ChangePassword BadRequest ,idUser:{0}", idUser));
				return BadRequest(ModelState);
			}

			IdentityResult result = await _appUserManager.ChangePasswordAsync(User.Identity.GetUserId<int>(), model.OldPassword, model.NewPassword);

			if (!result.Succeeded)
			{
				_logger.Debug(string.Format("ChangePassword GetErrorResult,idUser:{0}", idUser));
				return GetErrorResult(result);
			}

			_logger.Debug(string.Format("ChangePassword Ok ,idUser:{0}", idUser));

			return Ok(new JsonResponse { Success = true, Message = "Password changed, Please login again" });

		}

		// POST api/Account/SetPassword
		[System.Web.Http.Route("SetPassword")]
		public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model)
		{
			_logger.Debug("ini SetPassword process");
			if (!ModelState.IsValid)
			{
				_logger.Debug("ini SetPassword BadRequest");
				return BadRequest(ModelState);
			}

			IdentityResult result = await _appUserManager.AddPasswordAsync(User.Identity.GetUserId<int>(), model.NewPassword);

			if (!result.Succeeded)
			{
				_logger.Debug("ini SetPassword GetErrorResult");
				return GetErrorResult(result);
			}

			_logger.Debug("ini SetPassword Ok");

			return Ok(new JsonResponse { Success = true, Message = "Password setted" });
		}

		//
		// POST: /Account/ForgotPassword
		[System.Web.Http.HttpPost]
		public async Task<IHttpActionResult> ForgotPassword(ForgotPasswordBindingModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await _appUserManager.FindByEmailAsync(model.Email);
				
				if (user == null || !(await _appUserManager.IsEmailConfirmedAsync(user.Id)))
				{
					// Don't reveal that the user does not exist
					return BadRequest("Something is wrong,please try again later");
				}

				string generatedCode = await _appUserManager.GeneratePasswordResetTokenAsync(user.Id);
				
				var callbackUrl = string.Format(@"{0}/{1}", model.ResetURL, HttpUtility.UrlEncode(generatedCode));

				 string bodyHtmlEmail = Helpers.CreateBodyemailForgotPassword(user, callbackUrl);

				await _appUserManager.SendEmailAsync(user.Id, "Reset Password", bodyHtmlEmail);

				return Ok(new JsonResponse { Success = true, Message = "Please check your email to reset your password." });

			}

		   return BadRequest(ModelState);
		}

		[System.Web.Http.AllowAnonymous]
		[System.Web.Http.Route("ResetPassword")]
		[System.Web.Http.HttpPost]
		public async Task<IHttpActionResult> ResetPassword(ResetPasswordBindingModel model)
		{
			string message = "Error ResetPassword."; 

			if (!ModelState.IsValid)
			{
				// Don't reveal that the user does not exist
				return BadRequest(message);
			}

			var user = await _appUserManager.FindByEmailAsync(model.Email);
			
			if (user == null)
			{
				// Don't reveal that the user does not exist
				return BadRequest(message);
			}

			var result = await _appUserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
			
			if (result.Succeeded)
			{
				return Ok(new JsonResponse { Success = true, Message = "Your password has been reset." });
			}

			foreach (var error in result.Errors)
			{
				ModelState.AddModelError("",error);
			}

			return BadRequest(ModelState);
		}

		[System.Web.Http.OverrideAuthentication]
		[HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
		[System.Web.Http.AllowAnonymous]
		[System.Web.Http.Route("ExternalLoginCallback", Name = "ExternalLoginCallback")]
		[System.Web.Http.HttpGet]
		public async Task<IHttpActionResult> ExternalLoginCallback(string provider)
		{
			_logger.Debug( string.Format("ini ExternalLoginCallback process - provider:{0}",provider));

		   string redirectUri = string.Empty;

			if (!User.Identity.IsAuthenticated)
			{
				_logger.Debug("ExternalLoginCallback IsAuthenticated");
				return new ChallengeResult(provider, this);
			}

			var redirectUriValidationResult = Helpers.ValidateClientAndRedirectUri(ref redirectUri,Request);

			if (!string.IsNullOrWhiteSpace(redirectUriValidationResult))
			{
				_logger.Debug("ExternalLoginCallback BadRequest");
				return BadRequest(redirectUriValidationResult);
			}

			ExternalLoginModel externalLogin = ExternalLoginModel.FromIdentity(User.Identity as ClaimsIdentity);

			if (externalLogin == null)
			{
				_logger.Debug("ExternalLoginCallback InternalServerError");
				return InternalServerError();
			}

			if (externalLogin.LoginProvider != provider)
			{
				_logger.Debug("ExternalLoginCallback SignOut different providers");
				Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
				return new ChallengeResult(provider, this);
			}

			User user = await _appUserManager.FindAsync(new UserLoginInfo(externalLogin.LoginProvider, externalLogin.ProviderKey));

			bool hasRegistered = user != null;

			if (hasRegistered && user.Disabled)
			{
				return BadRequest("Your account is disabled, please contact with the web master");
			}

			redirectUri = string.Format("{0}?externalAccessToken={1}&provider={2}&haslocalaccount={3}&userName={4}&email={5}",
											redirectUri,externalLogin.ExternalAccessToken,externalLogin.LoginProvider,
											hasRegistered, externalLogin.UserName, externalLogin.Email);
			_logger.Debug(string.Format("ExternalLoginCallback Redirect info provider:{0},hasRegistered:{1},externalLogin.UserName:{2},externalLogin.Email{3}", provider, hasRegistered, externalLogin.UserName, externalLogin.Email));
			return Redirect(redirectUri);
		}

		// POST api/Account/Register
		[System.Web.Http.AllowAnonymous]
		[System.Web.Http.Route("Register")]
		[System.Web.Http.HttpPost]
		public async Task<IHttpActionResult> Register(RegisterBindingModel model)
		{
			_logger.Debug("ini Register - process");
			try
			{
				if (!ModelState.IsValid)
				{
					_logger.Debug("ini Register - inValid");
					return BadRequest(ModelState);
				}

				var user = new User
				{
					UserName = model.UserName,
					Email = model.Email
				};

				IdentityResult addUserResult = await _appUserManager.CreateAsync(user, model.Password);

				if (!addUserResult.Succeeded)
				{
					_logger.Debug("ini Register - GetErrorResult");
					return GetErrorResult(addUserResult);
				}

				_logger.Debug(string.Format("user register info email:{0}, idUser:{1}",user.Email,user.Id));

				_appUserManager.AddToRole(user.Id, "member");
				_appUserManager.AddClaim(user.Id, new Claim(ClaimTypes.Role, "member"));
				_appUserManager.AddClaim(user.Id, new Claim(ClaimTypes.Authentication, "local"));

				string generatedCode = await _appUserManager.GenerateEmailConfirmationTokenAsync(user.Id);

				Uri callbackUrl = new Uri(Url.Link("ConfirmEmailRoute", new { idUser = user.Id, code = generatedCode, confirmURL = Helpers.Encrypt(model.ConfirmURL) }));

				string bodyHtmlEmail = Helpers.CreateBodyEmail(user, callbackUrl.ToString());

				await _appUserManager.SendEmailAsync(user.Id, "Confirm your account", bodyHtmlEmail);

				Uri locationHeader = new Uri(Url.Link("GetUserById", new { id = user.Id }));

				_logger.Debug("ini Register - Success");
				_appUserManager.Update(user);
				return Created(locationHeader,ModelFactory.Create(user));
			}
			catch (Exception ex)
			{
				LogError(ex);
				return InternalServerError(ex);
			}
		}

		// POST api/Account/LogIn
		[System.Web.Http.AllowAnonymous]
		[System.Web.Http.Route("LogIn")]
		[System.Web.Http.HttpPost]
		public async Task<IHttpActionResult> LogIn(LoginBindingModel model)
		{
			try
			{
				_logger.Debug("ini LogIn - process");

				if (ModelState.IsValid)
				{
					User user = await _appUserManager.FindByEmailAsync(model.Email);

					if (user != null)
					{
						if (user.Disabled)
						{
							ModelState.AddModelError("", "Your account is disabled, please contact with the web master");
							return BadRequest(ModelState);
						}

						var validCredentials = await _appUserManager.FindAsync(user.UserName, model.Password);

						// When a user is lockedout, this check is done to ensure that even if the credentials are valid
						// the user can not login until the lockout duration has passed
						if (await _appUserManager.IsLockedOutAsync(user.Id))
						{
							ModelState.AddModelError("", string.Format(
								"Your account has been locked out for {0} minutes due to multiple failed login attempts.",
								ConfigurationManager.AppSettings["DefaultAccountLockoutTimeSpan"]));
						}
						// if user is subject to lockouts and the credentials are invalid
						// record the failure and check if user is lockedout and display message, otherwise,
						// display the number of attempts remaining before lockout
						else if (await _appUserManager.GetLockoutEnabledAsync(user.Id) && validCredentials == null)
						{
							// Record the failure which also may cause the user to be locked out
							await _appUserManager.AccessFailedAsync(user.Id);

							string message;

							if (await _appUserManager.IsLockedOutAsync(user.Id))
							{
								message =
									string.Format(
										"Your account has been locked out for {0} minutes due to multiple failed login attempts.",
										ConfigurationManager.AppSettings["DefaultAccountLockoutTimeSpan"]);
							}
							else
							{
								int accessFailedCount = await _appUserManager.GetAccessFailedCountAsync(user.Id);

								int attemptsLeft =
									Convert.ToInt32(
										ConfigurationManager.AppSettings["MaxFailedAccessAttemptsBeforeLockout"]) -
									accessFailedCount;

								message =
									string.Format(
										"Invalid credentials. You have {0} more attempt(s) before your account gets locked out..",
										attemptsLeft);

							}

							ModelState.AddModelError("", message);
						}
						else if (validCredentials == null)
						{
							ModelState.AddModelError("", "Invalid credentials. Please try again.");
						}
						else
						{
							await _appUserManager.ResetAccessFailedCountAsync(user.Id);

							var signInStatus = await  _appSignInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, false);
						   
							switch (signInStatus)
							{
								case SignInStatus.Success:
								{
									_logger.Debug("LogIn - Success");
									_logger.Debug(string.Format("user LogIn info email:{0}, idUser:{1}", user.Email, user.Id));
									return Ok(ModelFactory.Create(user));
								}

								case SignInStatus.Failure:
								{
									_logger.Debug("LogIn - Invalid password");
									ModelState.AddModelError("", "Invalid credentials. Please try again");
									break;
								}
							}
						}

					}
					else
					{
						_logger.Debug("LogIn - invalid email.");
						ModelState.AddModelError("", "Invalid credentials. Please try again");
					}
				}

				_logger.Debug("LogIn - BadRequest.");
				return BadRequest(ModelState);
			}
			catch (Exception ex)
			{
				LogError(ex);
				return InternalServerError(ex);
			}
		}

		[System.Web.Http.Route("GetUser", Name = "GetUserById")]
		public async Task<IHttpActionResult> GetUser(int id)
		{
			_logger.Debug(string.Format("ini GetUser - process, idUser:{0}",id));
			var user = await _appUserManager.FindByIdAsync(id);

			if (user != null)
			{
				_logger.Debug(string.Format("GetUser - Ok, idUser:{0}",id));
				return Ok(ModelFactory.Create(user));
			}
			_logger.Debug(string.Format("GetUser - NotFound, idUser:{0}", id));
			return NotFound();

		}

		[System.Web.Http.AllowAnonymous]
		[System.Web.Http.HttpGet]
		[System.Web.Http.Route("ConfirmEmail", Name = "ConfirmEmailRoute")]
		public async Task<IHttpActionResult> ConfirmEmail(int idUser, string code)
		{
			_logger.Debug(string.Format("ini ConfirmEmail - process ,idUser:{0}",idUser));

			string status = "fail";

			if (idUser == 0 || string.IsNullOrWhiteSpace(code))
			{
				status = "User Id and Code are required";
				_logger.Debug(string.Format("ConfirmEmail - status:{0},idUser:{1}", status, idUser));
			}

			IdentityResult result = await _appUserManager.ConfirmEmailAsync(idUser, code);

			if(result.Succeeded)
            {
                status = "success";
                _logger.Debug(string.Format("ConfirmEmail - status:{0},idUser:{1}", status, idUser));
                return Ok(new JsonResponse { Success = true, Message = "The verification was successful" });

            }else
            {
                status = "failed";
                _logger.Debug(string.Format("ConfirmEmail - status:{0},idUser:{1}", status, idUser));
                return Ok(new JsonResponse { Success = false, Message = "Error, please try later" });
            }
		}

		[System.Web.Http.AllowAnonymous]
		[System.Web.Http.Route("RegisterLoginExternal")]
		[System.Web.Http.HttpPost]
		public async Task<IHttpActionResult> RegisterLoginExternal(RegisterExternalBindingModel model)
		{
			_logger.Debug("ini RegisterLoginExternal - process");
			return await (model.HasLocalAccount? ObtainLocalAccessToken(model):RegisterExternal(model));
		}

		[System.Web.Http.HttpGet]
		public async Task<IHttpActionResult> ValidateEmail(string email)
		{
			_logger.Debug(string.Format("ini ValidateEmail - process, email:{0}",email));
			try
			{
				JsonResponse jsonResponse = new JsonResponse { Success = true };

				User user = await _appUserManager.FindByEmailAsync(email);
				jsonResponse.Success = (user == null);

				_logger.Debug(string.Format("ValidateEmail - OK, email:{0}", email));
				return Ok(jsonResponse);
			}
			catch (Exception ex)
			{
				LogError(ex);
				return InternalServerError(ex);
			}
		}

		[System.Web.Http.HttpGet]
		public  IHttpActionResult ValidateUserName(string userName)
		{
			_logger.Debug(string.Format("ini ValidateUserName - process, userName:{0}", userName));
			try
			{
				JsonResponse jsonResponse = new JsonResponse { Success = true };

				User user = _appUserManager.FindByName(userName);
				jsonResponse.Success = (user == null);

				_logger.Debug(string.Format("ValidateUserName - OK, userName:{0}", userName));
				return Ok(jsonResponse);
			}
			catch (Exception ex)
			{
				LogError(ex);
				return InternalServerError(ex);
			}
		}

		private async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model)
		{
			_logger.Debug("ini RegisterExternal - process");
		   
			if (!ModelState.IsValid)
			{
				_logger.Debug("RegisterExternal - ModelState inIsValid");
				return BadRequest(ModelState);
			}

			var verifiedAccessToken = await Helpers.VerifyExternalAccessToken(model.Provider, model.ExternalAccessToken);
		   
			if (verifiedAccessToken == null)
			{
				_logger.Debug("RegisterExternal - Invalid Provider or External Access Token");
				return BadRequest("Invalid Provider or External Access Token");
			}

			User user = await _appUserManager.FindAsync(new UserLoginInfo(model.Provider, verifiedAccessToken.user_id));

			bool hasRegistered = user != null;

			if (hasRegistered)
			{
				_logger.Debug("RegisterExternal - External user is already registered");
				return BadRequest("External user is already registered");
			}

			user = new User { UserName = model.UserName, Email = model.Email};

			IdentityResult result = await _appUserManager.CreateAsync(user);
		   
			if (!result.Succeeded)
			{
				_logger.Debug("RegisterExternal -CreateAsync- result fail");
				return GetErrorResult(result);
			}

			_appUserManager.AddToRole(user.Id, "member");
			_appUserManager.AddClaim(user.Id, new Claim(ClaimTypes.Authentication, model.Provider));

			var info = new ExternalLoginInfo
			{
				DefaultUserName = model.UserName, Login = new UserLoginInfo(model.Provider, verifiedAccessToken.user_id)
			};

			result = await _appUserManager.AddLoginAsync(user.Id, info.Login);
			
			if (!result.Succeeded)
			{
				_logger.Debug("RegisterExternal -AddLoginAsync- result fail");
				return GetErrorResult(result);
			}

			//generate access token response
			ClaimsIdentity identity = _appUserManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);

			var accessTokenResponse = Helpers.GenerateLocalAccessTokenResponse(user.UserName, identity);

			_logger.Debug(string.Format("RegisterExternal - Ok,idUser:{0},email:{1}", user.Id, user.Email));

			_appUserManager.Update(user);
			return Ok(accessTokenResponse);
		}

		private async Task<IHttpActionResult> ObtainLocalAccessToken(RegisterExternalBindingModel model)
		{
			_logger.Debug("ini ObtainLocalAccessToken - process");
			if (string.IsNullOrWhiteSpace(model.Provider) || string.IsNullOrWhiteSpace(model.ExternalAccessToken))
			{
				_logger.Debug("ObtainLocalAccessToken - Provider or external access token is not sent");
				return BadRequest("Provider or external access token is not sent");
			}

			var verifiedAccessToken = await Helpers.VerifyExternalAccessToken(model.Provider, model.ExternalAccessToken);
			if (verifiedAccessToken == null)
			{
				_logger.Debug("ObtainLocalAccessToken - invalid Provider or External Access Token");
				return BadRequest("Invalid Provider or External Access Token");
			}

			User user = await _appUserManager.FindAsync(new UserLoginInfo(model.Provider, verifiedAccessToken.user_id));

			bool hasRegistered = user != null;

			if (!hasRegistered)
			{
				_logger.Debug("ObtainLocalAccessToken - External user is not registered");
				return BadRequest("External user is not registered");
			}

			//generate access token response
			ClaimsIdentity identity = _appUserManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);

			var accessTokenResponse = Helpers.GenerateLocalAccessTokenResponse(user.UserName, identity);
			_logger.Debug(string.Format("ObtainLocalAccessToken - Ok,idUser:{0},email:{1}",user.Id,user.Email));
			_appUserManager.Update(user);
			return Ok(accessTokenResponse);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				//_userManager.Dispose();
			}

			base.Dispose(disposing);
		}

		#region Aplicaciones auxiliares

		#endregion
	}
}