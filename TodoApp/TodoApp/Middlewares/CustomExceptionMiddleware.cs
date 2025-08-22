using FluentValidation;
using System.Net;
using System.Text.Json;
using TodoApp.Models.Common;
using TodoApp.Utiities.Exceptions;

namespace TodoApp.Middlewares
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }


        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            string message = ex.Message ?? "An error occurred while processing your request.";
            string? errorMessage = ex.InnerException?.Message ?? ex.Message;

            // Not Found Exception
            if (ex is NotFoundException)
            {
                statusCode = HttpStatusCode.NotFound;
                message = ex.Message ?? "The requested resource was not found.";
            }

            // Bad Request Exception
            if (ex is BadRequestException)
            {
                statusCode = HttpStatusCode.BadRequest;
                message = ex.Message ?? "The request was invalid. Check validation errors for more information.";
            }

            if (ex is UnauthorizedException)
            {
                statusCode = HttpStatusCode.Unauthorized;
                message = ex.Message ?? "You are not authorized to perform this action.";
            }

            if (ex is ValidationException validationEx)
            {
                statusCode = HttpStatusCode.BadRequest;
                var errors = validationEx.Errors
                    .Select(e => e.ErrorMessage)
                    .ToList();
                var errorData = new { Errors = errors };
                var validationResponse = new ApiResponse<object>(errorData, false, "Validation failed");
                return WriteResponse(context, statusCode, validationResponse);
            }

            ErrorResponse error = new()
            {
                StatusCode = (int)statusCode,
                ErrorMessage = errorMessage ?? "Unknown error occurred."
            };

            ApiResponse<ErrorResponse> apiResponse = new(error, false, message);

            string responseJson = JsonSerializer.Serialize(apiResponse);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            return context.Response.WriteAsync(responseJson);
        }
        private static Task WriteResponse(HttpContext context, HttpStatusCode statusCode, object responseObj)
        {
            string responseJson = JsonSerializer.Serialize(responseObj);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            return context.Response.WriteAsync(responseJson);
        }
    }
}

