using FluentValidation;
using log4net;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Net;
using Todo.API.Dtos;
using Todo.API.Helpers;

namespace Todo.API.MIddlewares
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly ILog _log;
        private readonly RequestDelegate _next;

        public GlobalExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
            _log = LogManager.GetLogger(typeof(GlobalExceptionHandlingMiddleware));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                context.Response.ContentType = Utils.JsonContentType;

                await _next(context);
            }
            catch (ValidationException ex)
            {
                await HandleValidationExceptionAsync(context, ex, context.RequestAborted);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, context.RequestAborted);
            }
        }


        private Task HandleValidationExceptionAsync(HttpContext httpContext, ValidationException ex, CancellationToken token = default)
        {
            ApiResponse response = new ()
            {
                StatusCode = HttpStatusCode.BadRequest,
                Message = "Request validation error!",
                Errors = ex.Errors.Select(x => new ValidationResponse
                {
                    PropertyName = x.PropertyName,
                    Message = x.ErrorMessage
                })
            };


            httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return httpContext.Response.WriteAsJsonAsync(response, token);
        }

        private Task HandleExceptionAsync(HttpContext httpContext, Exception ex, CancellationToken token = default)
        {
            var endpoint = httpContext.GetEndpoint();
            var controllerName = endpoint?.Metadata.GetMetadata<ControllerActionDescriptor>()?.ControllerName;
            var actionName = endpoint?.Metadata.GetMetadata<ControllerActionDescriptor>()?.ActionName;

            _log.Error($"GlobalExceptionHandlingMiddleware.{controllerName}.{actionName}.Error  is : " + ex);

            ApiResponse response = new()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Message = "Failed to execute the request, error: " + ex.Message,
                Errors = Enumerable.Empty<ValidationResponse>()
            };

            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return httpContext.Response.WriteAsJsonAsync(response, token);
        }
    }
}
