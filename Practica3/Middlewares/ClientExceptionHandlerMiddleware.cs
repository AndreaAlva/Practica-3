using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExternalCServices.Exceptions;
using ClientLogic.Exceptions;
using System.Net;
using Newtonsoft.Json;
using Serilog;

namespace Practica3.Middlewares
{
    class ClientExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        public ClientExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch(Exception e)
            {
                await HandlingExceptions(httpContext,e);
            }
            
        }

        private Task HandlingExceptions(HttpContext httpContext, Exception e)
        {
            string message="";
            int code = (int)HttpStatusCode.OK;
            
            if(e is ExternalClientServiceNotFoundException)
            {
                message = "External Backing Service error: "+e.Message;
            }
            else if (e is ClientInvalidInputException)
            {
                message = "Invalid input data: " + e.Message;
            }
            else if(e is ClientDatabaseException)
            {
                message = "Database error: "+ e.Message;
            }
            else if (e is ClientNotFoundException)
            {
                message = "Client error: "+ e.Message;
            }
            else if (e.InnerException is ExternalClientServiceException)
            {
                message = e.Message;
            }
            else
            {
                message = e.Message;
                code = (int)HttpStatusCode.InternalServerError;
            }
            var response = new { message, code };
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = code;
            Log.Error(message);
            return httpContext.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }
    public static class ClientExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseClientExceptionHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ClientExceptionHandlerMiddleware>();
        }
    }
}
