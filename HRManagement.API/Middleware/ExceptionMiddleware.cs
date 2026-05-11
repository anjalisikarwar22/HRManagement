using HRManagement.API.Common;
using HRManagement.API.Exceptions;
using System.Net;
using System.Text.Json;

namespace HRManagement.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(
            RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(
            HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(
                    context,
                    ex);
            }
        }

        private static Task HandleExceptionAsync(
            HttpContext context,
            Exception exception)
        {
            HttpStatusCode statusCode;

            switch (exception)
            {
                case BadRequestException:
                    statusCode =
                        HttpStatusCode.BadRequest;
                    break;

                case NotFoundException:
                    statusCode =
                        HttpStatusCode.NotFound;
                    break;

                case UnauthorizedException:
                    statusCode =
                        HttpStatusCode.Unauthorized;
                    break;

                case ForbiddenException:
                    statusCode =
                        HttpStatusCode.Forbidden;
                    break;

                case ValidationException:
                    statusCode =
                        HttpStatusCode.BadRequest;
                    break;

                default:
                    statusCode =
                        HttpStatusCode
                        .InternalServerError;
                    break;
            }

            var response =
                new ApiResponse<object>(
                    false,
                    exception.Message,
                    null);

            var jsonResponse =
                JsonSerializer.Serialize(response);

            context.Response.ContentType =
                "application/json";

            context.Response.StatusCode =
                (int)statusCode;

            return context.Response
                .WriteAsync(jsonResponse);
        }
    }

}