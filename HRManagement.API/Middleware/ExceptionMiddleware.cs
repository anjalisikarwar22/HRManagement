using HRManagement.API.Common;
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
                await WriteErrorAsync(context, HttpStatusCode.BadRequest, ex.Message);
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
        public ExceptionMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext ctx)
        {
            try
            {
                await _next(ctx);
            }
            catch (NotFoundException ex)
            {
                await Write(ctx, HttpStatusCode.NotFound, ex.Message);
            }
            catch (ValidationException ex)
            {
                await Write(ctx, HttpStatusCode.BadRequest, string.Join(" | ", ex.Errors));
            }
            catch (BadRequestException ex)
            {
                await Write(ctx, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (DuplicateException ex)
            {
                await Write(ctx, HttpStatusCode.Conflict, ex.Message);
            }
            catch (UnauthorizedException ex)
            {
                await Write(ctx, HttpStatusCode.Unauthorized, ex.Message);
            }
            catch (ForbiddenException ex)
            {
                await Write(ctx, HttpStatusCode.Forbidden, ex.Message);
            }
            catch (Exception ex)
            {
                await Write(ctx, HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        private static Task Write(HttpContext ctx, HttpStatusCode status, string message)
        {
            ctx.Response.StatusCode = (int)status;
            ctx.Response.ContentType = "application/json";
            var body = new ApiResponse<object>(false, message, null);
            return ctx.Response.WriteAsync(JsonSerializer.Serialize(body));
        }
    }
}
