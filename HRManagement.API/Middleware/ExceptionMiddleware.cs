using HRManagement.API.Common;
using HRManagement.API.Exceptions;
using System.Net;
using System.Text.Json;

namespace HRManagement.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (NotFoundException ex)
            {
                await WriteErrorAsync(context, HttpStatusCode.NotFound, ex.Message);
            }
            catch (DuplicateException ex)
            {
                await WriteErrorAsync(context, HttpStatusCode.Conflict, ex.Message);
            }
            catch (BadRequestException ex)
            {
                await WriteErrorAsync(context, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (UnauthorizedException ex)
            {
                await WriteErrorAsync(context, HttpStatusCode.Unauthorized, ex.Message);
            }
            catch (ForbiddenException ex)
            {
                await WriteErrorAsync(context, HttpStatusCode.Forbidden, ex.Message);
            }
            catch (ValidationException ex)
            {
                var message = ex.Errors != null && ex.Errors.Any()
                    ? string.Join(" | ", ex.Errors)
                    : ex.Message;
                await WriteErrorAsync(context, HttpStatusCode.BadRequest, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error");
                await WriteErrorAsync(
                    context,
                    HttpStatusCode.InternalServerError,
                    "Something went wrong. Please try again.");
            }
        }

        private static async Task WriteErrorAsync(
            HttpContext context,
            HttpStatusCode statusCode,
            string message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var response = ApiResponse<object>.FailureResponse(message);
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
