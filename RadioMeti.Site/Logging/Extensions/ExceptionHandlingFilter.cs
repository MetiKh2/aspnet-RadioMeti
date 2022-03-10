using Microsoft.AspNetCore.Mvc.Filters;

namespace RadioMeti.Site.Logging.Extensions
{
    public class ExceptionHandlingFilter: ExceptionFilterAttribute
    {
        private readonly ILogger<ExceptionHandlingFilter> _logger;
        public ExceptionHandlingFilter()
        {
            var loggerFactory = new LoggerFactory();
            _logger = loggerFactory.CreateLogger<ExceptionHandlingFilter>();
        }


        public override void OnException(ExceptionContext context)
        {
            _logger.LogError($"OnException() - {context.Exception.Message}");
            context.ExceptionHandled = true;
        }
    }
}
