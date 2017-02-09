namespace Blog.WebApiSite.Core
{
    using System.Web.Http.ExceptionHandling;
    using log4net;

    public class Log4NetExceptionLogger : ExceptionLogger
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(Log4NetExceptionLogger));

        public override void Log(ExceptionLoggerContext context)
        {
            _logger.ErrorFormat("Error requesting {0}. Error : {1}", context.Request.RequestUri.AbsoluteUri, context.Exception);
        }
    }
}