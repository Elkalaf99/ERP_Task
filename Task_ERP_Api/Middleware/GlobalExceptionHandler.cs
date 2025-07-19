using System.Net;
using System.Text.Json;

namespace Task_ERP_Api.Middleware
{
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger)
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
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = new
            {
                error = new
                {
                    message = GetUserFriendlyMessage(exception),
                    type = exception.GetType().Name,
                    timestamp = DateTime.UtcNow
                }
            };

            switch (exception)
            {
                case ArgumentNullException:
                case ArgumentException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;

                case InvalidOperationException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;

                case UnauthorizedAccessException:
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    break;

                case KeyNotFoundException:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;

                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response = new
                    {
                        error = new
                        {
                            message = "An unexpected error occurred. Please try again later.",
                            type = "InternalServerError",
                            timestamp = DateTime.UtcNow
                        }
                    };
                    break;
            }

            _logger.LogError(exception, "An error occurred: {Message}", exception.Message);

            var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(jsonResponse);
        }

        private static string GetUserFriendlyMessage(Exception exception)
        {
            return exception switch
            {
                ArgumentNullException argNullEx => argNullEx.Message,
                ArgumentException argEx => argEx.Message,
                InvalidOperationException invalidOpEx => invalidOpEx.Message,
                UnauthorizedAccessException unauthorizedEx => "You are not authorized to perform this action.",
                KeyNotFoundException keyNotFoundEx => "The requested resource was not found.",
                _ => "An unexpected error occurred. Please try again later."
            };
        }
    }

    public static class GlobalExceptionHandlerExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GlobalExceptionHandler>();
        }
    }
} 