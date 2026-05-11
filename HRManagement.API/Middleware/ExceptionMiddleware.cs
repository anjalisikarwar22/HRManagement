using HRManagement.API.Common;
using HRManagement.API.Exceptions;
using System.Net;
using System.Text.Json;

namespace HRManagement.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

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
