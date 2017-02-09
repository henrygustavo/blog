namespace Blog.WebApiSite.Core
{
	using System;
	using System.Collections.Generic;
	using System.Configuration;
	using System.IO;
	using System.Linq;
	using System.Net.Http;
	using System.Security.Claims;
	using System.Security.Cryptography;
	using System.Text;
	using System.Threading.Tasks;
	using System.Web.Mvc;
	using Models;
	using Microsoft.Owin.Security;
	using Newtonsoft.Json.Linq;
	using System.Web;
	using Business.Entity;

	public  static class Helpers
	{
		public static string ValidateClientAndRedirectUri(ref string redirectUriOutput, HttpRequestMessage request)
		{
			Uri redirectUri;

			var redirectUriString = GetQueryString(request, "redirect_uri");

			if (string.IsNullOrWhiteSpace(redirectUriString))
			{
				return "redirect_uri is required";
			}

			bool validUri = Uri.TryCreate(redirectUriString, UriKind.Absolute, out redirectUri);

			if (!validUri)
			{
				return "redirect_uri is invalid";
			}

			redirectUriOutput = redirectUri.AbsoluteUri;

			return string.Empty;
		}

		public static async Task<ParsedExternalAccessToken> VerifyExternalAccessToken(string provider, string accessToken)
		{
			ParsedExternalAccessToken parsedToken = null;

			var verifyTokenEndPoint = "";

			if (provider == "Facebook")
			{
				//You can get it from here: https://developers.facebook.com/tools/accesstoken/
				//More about debug_tokn here: http://stackoverflow.com/questions/16641083/how-does-one-get-the-app-access-token-for-debug-token-inspection-on-facebook
				const string appToken = "1003393039680040|8J8UU40B0EM0xnzc02hQonNlRVU";
				verifyTokenEndPoint = string.Format("https://graph.facebook.com/debug_token?input_token={0}&access_token={1}", accessToken, appToken);
			}
			else if (provider == "Google")
			{
				verifyTokenEndPoint = string.Format("https://www.googleapis.com/oauth2/v1/tokeninfo?access_token={0}", accessToken);
			}
			else
			{
				return null;
			}

			var client = new HttpClient();
			var uri = new Uri(verifyTokenEndPoint);
			var response = await client.GetAsync(uri);

			if (response.IsSuccessStatusCode)
			{
				var content = await response.Content.ReadAsStringAsync();

				dynamic jObj = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content);

				parsedToken = new ParsedExternalAccessToken();

				if (provider == "Facebook")
				{
					parsedToken.user_id = jObj["data"]["user_id"];
					parsedToken.app_id = jObj["data"]["app_id"];

					if (!string.Equals(Startup.FacebookAuthOptions.AppId, parsedToken.app_id, StringComparison.OrdinalIgnoreCase))
					{
						return null;
					}
				}
				else if (provider == "Google")
				{
					parsedToken.user_id = jObj["user_id"];
					parsedToken.app_id = jObj["audience"];

					if (!string.Equals(Startup.GoogleAuthOptions.ClientId, parsedToken.app_id, StringComparison.OrdinalIgnoreCase))
					{
						return null;
					}
				}
			}

			return parsedToken;
		}

		public static JObject GenerateLocalAccessTokenResponse(string userName, ClaimsIdentity identity)
		{
			var tokenExpiration = TimeSpan.FromDays(1);

			var props = new AuthenticationProperties()
			{
				IssuedUtc = DateTime.UtcNow,
				ExpiresUtc = DateTime.UtcNow.Add(tokenExpiration)
			};

			var ticket = new AuthenticationTicket(identity, props);

			var accessToken = Startup.OAuthServerOptions.AccessTokenFormat.Protect(ticket);

			JObject tokenResponse = new JObject(
										new JProperty("userName", userName),
										new JProperty("access_token", accessToken),
										new JProperty("token_type", "bearer"),
										new JProperty("expires_in", tokenExpiration.TotalSeconds.ToString()),
										new JProperty(".issued", ticket.Properties.IssuedUtc.ToString()),
										new JProperty(".expires", ticket.Properties.ExpiresUtc.ToString())
		);

			return tokenResponse;
		}

		public static string Encrypt(string clearText)
		{
			string encryptionKey = ConfigurationManager.AppSettings["encryptionKey"];
			byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
			using (Aes encryptor = Aes.Create())
			{
				Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
				encryptor.Key = pdb.GetBytes(32);
				encryptor.IV = pdb.GetBytes(16);
				using (MemoryStream ms = new MemoryStream())
				{
					using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
					{
						cs.Write(clearBytes, 0, clearBytes.Length);
						cs.Close();
					}
					clearText = Convert.ToBase64String(ms.ToArray());
				}
			}
			return clearText;
		}

		public  static string Decrypt(string cipherText)
		{
			string encryptionKey = ConfigurationManager.AppSettings["encryptionKey"];
			cipherText = cipherText.Replace(" ", "+");
			byte[] cipherBytes = Convert.FromBase64String(cipherText);
			using (Aes encryptor = Aes.Create())
			{
				Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
				encryptor.Key = pdb.GetBytes(32);
				encryptor.IV = pdb.GetBytes(16);
				using (MemoryStream ms = new MemoryStream())
				{
					using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
					{
						cs.Write(cipherBytes, 0, cipherBytes.Length);
						cs.Close();
					}
					cipherText = Encoding.Unicode.GetString(ms.ToArray());
				}
			}
			return cipherText;
		}

		public static List<SelectListItem> ConvertToListItem<T>(IList<T> list, string value, string text)
		{
			var listItems = (from entity in list
							 let propiedad1 = entity.GetType().GetProperty(value)
							 where propiedad1 != null
							 let valor1 = propiedad1.GetValue(entity, null)
							 where valor1 != null
							 let propiedad2 = entity.GetType().GetProperty(text)
							 where propiedad2 != null
							 let valor2 = propiedad2.GetValue(entity, null)
							 where valor2 != null
							 select new SelectListItem
							 {
								 Value = valor1.ToString(),
								 Text = valor2.ToString()
							 })
				.OrderBy(p => p.Text)
				.ToList();
			listItems.Insert(0, new SelectListItem { Text = "-- Select --", Value = "0" });
			return listItems;
		}

		public static List<Common> EnumToList<T>()
		{
			var enumType = typeof(T);

			if (enumType.BaseType != typeof(Enum))
				throw new ArgumentException("T debe ser de tipo System.Enum");

			var enumValArray = Enum.GetValues(enumType);
			var enumValList = (from object l in enumValArray
							   select new Business.Entity.Common
							   {
								   Key = Convert.ToString(l),
								   Value = Enum.GetName(enumType, l)
							   })
				.OrderBy(p => p.Key)
				.ToList();
			return enumValList;
		}

		public static string CreateBodyEmail(User user, string callbackUrl)
		{
			if(HttpContext.Current == null){ return string.Empty;}

			string bodyHtmlEmail = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/appAdmin/shared/html/emailVerification.html"));

			bodyHtmlEmail = bodyHtmlEmail.Replace("{{title}}", string.Format("Hi {0},", user.UserName));
			bodyHtmlEmail = bodyHtmlEmail.Replace("{{subTitle}}", "Thanks for signing up!");
			bodyHtmlEmail = bodyHtmlEmail.Replace("{{body}}", "Please confirm your account by clicking the button below");
			bodyHtmlEmail = bodyHtmlEmail.Replace("{{verifyUrl}}", callbackUrl);

			return bodyHtmlEmail;
		}

		public static string CreateBodyemailForgotPassword(User user, string callbackUrl)
		{
			if(HttpContext.Current == null){ return string.Empty;}

			string bodyHtmlEmail = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/appAdmin/shared/html/emailVerification.html"));
			bodyHtmlEmail = bodyHtmlEmail.Replace("{{title}}", string.Format("Hi {0},", user.UserName));
			bodyHtmlEmail = bodyHtmlEmail.Replace("{{subTitle}}", "Reset Password info.");
			bodyHtmlEmail = bodyHtmlEmail.Replace("{{body}}", "Please reset your password by clicking the button below");
			bodyHtmlEmail = bodyHtmlEmail.Replace("{{verifyUrl}}", callbackUrl);

			return bodyHtmlEmail;
		}

        /// <summary>
        /// Produces optional, URL-friendly version of a title, "like-this-one". 
        /// hand-tuned for speed, reflects performance refactoring contributed
        /// by John Gietzen (user otac0n) 
        /// </summary>
        public static string UrlFriendly(string title)
        {
            if (title == null) return "";

            const int maxlen = 80;
            int len = title.Length;
            bool prevdash = false;
            var sb = new StringBuilder(len);
            char c;

            for (int i = 0; i < len; i++)
            {
                c = title[i];
                if ((c >= 'a' && c <= 'z') || (c >= '0' && c <= '9'))
                {
                    sb.Append(c);
                    prevdash = false;
                }
                else if (c >= 'A' && c <= 'Z')
                {
                    // tricky way to convert to lowercase
                    sb.Append((char)(c | 32));
                    prevdash = false;
                }
                else if (c == ' ' || c == ',' || c == '.' || c == '/' ||
                    c == '\\' || c == '-' || c == '_' || c == '=')
                {
                    if (!prevdash && sb.Length > 0)
                    {
                        sb.Append('-');
                        prevdash = true;
                    }
                }
                else if (c >= 128)
                {
                    int prevlen = sb.Length;
                    sb.Append(RemapInternationalCharToAscii(c));
                    if (prevlen != sb.Length) prevdash = false;
                }
                if (i == maxlen) break;
            }

            if (prevdash)
                return sb.ToString().Substring(0, sb.Length - 1);
            return sb.ToString();
        }

        private static string RemapInternationalCharToAscii(char c)
        {
            string s = c.ToString().ToLowerInvariant();
            if ("àåáâäãåą".Contains(s))
            {
                return "a";
            }
            if ("èéêëę".Contains(s))
            {
                return "e";
            }
            if ("ìíîïı".Contains(s))
            {
                return "i";
            }
            if ("òóôõöøőð".Contains(s))
            {
                return "o";
            }
            if ("ùúûüŭů".Contains(s))
            {
                return "u";
            }
            if ("çćčĉ".Contains(s))
            {
                return "c";
            }
            if ("żźž".Contains(s))
            {
                return "z";
            }
            if ("śşšŝ".Contains(s))
            {
                return "s";
            }
            if ("ñń".Contains(s))
            {
                return "n";
            }
            if ("ýÿ".Contains(s))
            {
                return "y";
            }
            if ("ğĝ".Contains(s))
            {
                return "g";
            }
            if (c == 'ř')
            {
                return "r";
            }
            if (c == 'ł')
            {
                return "l";
            }
            if (c == 'đ')
            {
                return "d";
            }
            if (c == 'ß')
            {
                return "ss";
            }
            if (c == 'Þ')
            {
                return "th";
            }
            if (c == 'ĥ')
            {
                return "h";
            }
            if (c == 'ĵ')
            {
                return "j";
            }
            return string.Empty;
        }
        private static string GetQueryString(HttpRequestMessage request, string key)
		{
			var queryStrings = request.GetQueryNameValuePairs();

			if (queryStrings == null) return null;

			var match = queryStrings.FirstOrDefault(keyValue => String.Compare(keyValue.Key, key, StringComparison.OrdinalIgnoreCase) == 0);

			return string.IsNullOrEmpty(match.Value) ? null : match.Value;
		}

	}
}