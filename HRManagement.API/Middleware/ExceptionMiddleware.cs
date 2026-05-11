using HRManagement.API.Common;
﻿using HRManagement.API.Common;
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
            catch (Exception ex)
            {
                await Write(ctx, HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        private static Task Write(HttpContext ctx, HttpStatusCode status, string message)
        {
            ctx.Response.StatusCode = (int)status;
            ctx.Response.ContentType = "application/json";
            var body = new ApiResponse<object>
            {
                Success = false,
                Message = message,
                Data = null!
            };
            return ctx.Response.WriteAsync(JsonSerializer.Serialize(body));
        }
    }
}
    public class ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger)
    {
        private static readonly JsonSerializerOptions JsonOptions
            = new()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (NotFoundException ex)
            {
                logger.LogWarning(
                    "Not Found: {Message}", ex.Message);
                await SendErrorResponse(
                    context, ex.Message, HttpStatusCode.NotFound);
            }
            catch (DuplicateException ex)
            {
                logger.LogWarning(
                    "Duplicate: {Message}", ex.Message);
                await SendErrorResponse(
                    context, ex.Message, HttpStatusCode.Conflict);
            }
            catch (ValidationException ex)
            {
                logger.LogWarning(
                    "Validation: {Message}", ex.Message);
                await SendErrorResponse(
                    context, ex.Message,
                    HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex, "Unexpected Error: {Message}",
                    ex.Message);
                await SendErrorResponse(
                    context,
                    "Something went wrong. Please try again.",
                    HttpStatusCode.InternalServerError);
            }
        }
        private static async Task SendErrorResponse(
            HttpContext context,
            string message,
            HttpStatusCode statusCode)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var response = new ApiResponse<object>
            {
                Success = false,
                Message = message,
                Data = null
            };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(
                    response, JsonOptions));
        }
    }
}
