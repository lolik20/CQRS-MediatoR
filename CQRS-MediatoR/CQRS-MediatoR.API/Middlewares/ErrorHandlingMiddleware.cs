using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CQRS_MediatoR.Api.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly ILogger<ErrorHandlingMiddleware> logger;
        private readonly RequestDelegate next;
        /// <summary>
        /// Middleware for handling exceptions with proper response to client request
        /// </summary>
        /// <param name="next"></param>
        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            //catch (UserActionException ex)
            //{
            //    await HandleExceptionAsync(context, ex, HttpStatusCode.BadRequest);
            //}
            //catch (EntityNotFoundException ex)
            //{
            //    await HandleExceptionAsync(context, ex, HttpStatusCode.NotFound);
            //}
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex, HttpStatusCode code)
        {
            this.logger.LogError(ex, ex.ToString());

            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(ex.Message);
        }

	}
}
