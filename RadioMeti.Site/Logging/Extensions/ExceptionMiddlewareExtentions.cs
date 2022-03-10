using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;

namespace RadioMeti.Site.Logging.Extensions
{
    public static class ExceptionMiddlewareExtentions
    {
        public static void ConfigureBuildExceptionHandler(this IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            app.UseExceptionHandler(error =>
            {
                error.Run(async context =>
                {
                    var _logger = loggerFactory.CreateLogger("exceptionhandlermiddleware");

                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    var contextReuest = context.Features.Get<IHttpRequestFeature>();

                    if (contextFeature != null)
                    {
                        string _error = $"[{context.Response.StatusCode}] - {contextFeature.Error.Message}: {contextReuest.Path}";

                        _logger.LogError(_error);

                        await context.Response.WriteAsync(_error);

                    }

                });
            });
        }
    }
}
