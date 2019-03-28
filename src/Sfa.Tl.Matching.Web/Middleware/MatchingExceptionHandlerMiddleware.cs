using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

namespace Sfa.Tl.Matching.Web.Middleware
{
    public class MatchingExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly Func<object, Task> _clearCacheHeadersDelegate;

        public MatchingExceptionHandlerMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<ExceptionHandlerMiddleware>();
            _clearCacheHeadersDelegate = ClearCacheHeaders;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception e)
            {
                _logger.LogError(default, e, e.Message);

                if (httpContext.Response.HasStarted)
                    throw;

                var originalPath = httpContext.Request.Path;

                try
                {
                    httpContext.Response.Clear();
                    var exceptionHandlerFeature = new ExceptionHandlerFeature
                    {
                        Error = e,
                        Path = originalPath.Value,
                    };
                    httpContext.Features.Set<IExceptionHandlerFeature>(exceptionHandlerFeature);
                    httpContext.Features.Set<IExceptionHandlerPathFeature>(exceptionHandlerFeature);
                    httpContext.Response.StatusCode = 500;
                    httpContext.Response.OnStarting(_clearCacheHeadersDelegate, httpContext.Response);
                    httpContext.Request.Path = $"/Home/Error/{httpContext.Response.StatusCode}";

                    await _next(httpContext);
                }
                finally
                {
                    httpContext.Request.Path = originalPath;
                }
            }
        }

        private static Task ClearCacheHeaders(object state)
        {
            var response = (HttpResponse)state;
            response.Headers[HeaderNames.CacheControl] = "no-cache";
            response.Headers[HeaderNames.Pragma] = "no-cache";
            response.Headers[HeaderNames.Expires] = "-1";
            response.Headers.Remove(HeaderNames.ETag);
            return Task.CompletedTask;
        }
    }
}