using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Sfa.Tl.Matching.Web.Extensions.ApplicationBuilder
{
    public static class GlobalExceptionHandler
    {
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app,
            ILoggerFactory loggerFactory)
        {
            return app.UseExceptionHandler(error =>
            {
                error.Run(async context =>
                {
                    await Task.Run(() =>
                    {
                        var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();

                        if (exceptionHandlerFeature != null)
                        {
                            var logger = loggerFactory.CreateLogger("Global exception logger");
                            logger.LogError(500, exceptionHandlerFeature.Error, exceptionHandlerFeature.Error.Message);
                        }
                        context.Response.Redirect($"/Home/Error/{context.Response.StatusCode}");
                    });
                });
            });
        }
    }
}