namespace Blog.WebApiSite.Controllers
{
	using System;
	using System.Web.Http;
	using Common.Logging;
	using System.Collections.Generic;
	using Microsoft.AspNet.Identity;
	using Newtonsoft.Json;
	using Business.Entity;

	public class BaseController : ApiController
	{
		protected int CurrentIdUser
		{
			get { return(User != null) ? User.Identity.GetUserId<int>():0; }
		}

		private readonly ILog _logger;
		public BaseController(ILog logger)
		{
			_logger = logger;
		}
		protected void LogError(Exception exception)
		{
			_logger.Error(string.Format("Mensaje: {0} Trace: {1}", exception.Message, exception.StackTrace));
		}

		protected IHttpActionResult GetErrorResult(IdentityResult result)
		{
			if (result == null)
			{
				return InternalServerError();
			}

			if (!result.Succeeded)
			{
				if (result.Errors != null)
				{
					foreach (string error in result.Errors)
					{
						ModelState.AddModelError("", error);
					}
				}

				if (ModelState.IsValid)
				{
					// No hay disponibles errores ModelState para enviar, por lo que simplemente devuelva un BadRequest vacío.
					return BadRequest();
				}

				return BadRequest(ModelState);
			}

			return null;
		}

		protected List<FilterOption> ConvertJsonToDictionary(string json)
		{
		   List<FilterOption> obj = new List<FilterOption>();

		   if (!string.IsNullOrEmpty(json) && json != "{}")
			{
				obj = JsonConvert.DeserializeObject<List<FilterOption>>(json);
			}

			return obj;
		}
	}
}