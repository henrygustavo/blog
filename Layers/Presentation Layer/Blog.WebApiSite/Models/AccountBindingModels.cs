namespace Blog.WebApiSite.Models
{
	using System.ComponentModel.DataAnnotations;

	// Models used as parameters to AccountController actions.

	public class AddExternalLoginBindingModel
	{
		[Required]
		public string ExternalAccessToken { get; set; }
	}

	public class ExternalLoginBindingModel
	{
		public string Code { get; set; }
		public string ClientId { get; set; }

		public string RedirectUri { get; set; }
	}

	public class LoginBindingModel
	{
	   [Required]
	   [EmailAddress]
	   public string Email { get; set; }

	   [Required] 
	   [DataType(DataType.Password)]
	   public string Password { get; set; }

	   public bool RememberMe { get; set; }
	}

	public class ChangePasswordBindingModel
	{
		[Required]
		[DataType(DataType.Password)]
		public string OldPassword { get; set; }

		[Required]
		[StringLength(100, ErrorMessage = "the {0} must be at least {2} characters long.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		public string NewPassword { get; set; }

		[DataType(DataType.Password)]
		[Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }
	}

	public class RegisterBindingModel
	{
		[Required]
		public string UserName { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[StringLength(100, ErrorMessage = "the {0} must be at least {2} characters long.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "The password and confirmation password do not match..")]
		public string ConfirmPassword { get; set; }

		[Required]
		public string ConfirmURL { get; set; }
	}

	public class RegisterExternalBindingModel
	{
		[Required]
		public string UserName { get; set; }

		[Required]
		public string Provider { get; set; }

		[Required]
		public string ExternalAccessToken { get; set; }

		[Required]
		public string Email { get; set; }

		[Required]
		public bool HasLocalAccount { get; set; }
	}

	public class ParsedExternalAccessToken
	{
		public string user_id { get; set; }
		public string app_id { get; set; }
	}

	public class RemoveLoginBindingModel
	{
		[Required]
		public string LoginProvider { get; set; }

		[Required]
		public string ProviderKey { get; set; }
	}

	public class SetPasswordBindingModel
	{
		[Required]
		[StringLength(100, ErrorMessage = "the {0} must be at least {2} characters long.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		public string NewPassword { get; set; }

		[DataType(DataType.Password)]
		[Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }
	}

	public class ForgotPasswordBindingModel
	{
		[Required(ErrorMessage = "Email is required.")]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		public string ResetURL { get; set; }
	}

	public class ResetPasswordBindingModel
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[StringLength(100, ErrorMessage = "the {0} must be at least {2} characters long.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "The password and confirmation password do not match..")]
		public string ConfirmPassword { get; set; }

		public string Code { get; set; }
	}
}