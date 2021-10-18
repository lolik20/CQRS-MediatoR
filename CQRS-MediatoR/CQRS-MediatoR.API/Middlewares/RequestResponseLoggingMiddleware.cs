using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CQRS_MediatoR.Api.Middlewares
{
    public class RequestResponseLoggingMiddleware
    {

        private readonly RequestDelegate next;
        private readonly ILogger<RequestResponseLoggingMiddleware> logger;
        public RequestResponseLoggingMiddleware(RequestDelegate next,
            ILogger<RequestResponseLoggingMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            // await LogRequest(context);
            await LogResponse(context);
        }

        private async Task LogRequest(HttpContext context)
        {
            logger.LogInformation($"Http Request Information:{Environment.NewLine}" +
                                  $"Schema:{context.Request.Scheme} " +
                                  $"Method:{context.Request.Method} " +
                                  $"Host: {context.Request.Host} " +
                                  $"Path: {context.Request.Path} " +
                                  $"QueryString: {context.Request.QueryString} ");
        }

        private async Task LogResponse(HttpContext context)
        {
            await next(context);
            if (context.Response.StatusCode >= 400)
            {
                var wwwAuth = context.Response.Headers["www-authenticate"].ToString();
                var log = $" StatucCode: {context.Response.StatusCode},  " +
                          $"Http Response Information:{Environment.NewLine}" +
                          $"Schema:{context.Request.Scheme} " +
                          $"Method:{context.Request.Method} " +
                          $"Host: {context.Request.Host} " +
                          $"Path: {context.Request.Path} " +
                          $"QueryString: {context.Request.QueryString} " +
                          $" www-authenticate: {wwwAuth}";
                logger.LogWarning(log);
            }
        }

	}
    public static class RequestResponseLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestResponseLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestResponseLoggingMiddleware>();
        }
    }

}
