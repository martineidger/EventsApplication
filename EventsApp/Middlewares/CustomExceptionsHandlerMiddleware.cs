using FluentValidation;
using System.Net;
using System.Text.Json;

namespace EventsApp.Middlewares
{
    public class CustomExceptionsHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        public CustomExceptionsHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception ex) 
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            var result = string.Empty;
            switch(exception)
            {
                case ValidationException validationException:
                    code = HttpStatusCode.BadRequest;
                    result = $"Validation error: {string.Join(", ", validationException.Errors.Select(e => e.ErrorMessage))}";
                    break;
                case UnauthorizedAccessException unauthorizedAccessException:
                    code = HttpStatusCode.Unauthorized;
                    result = "Unauthorized";
                    break;
                case ArgumentNullException argumentNullException:
                    code = HttpStatusCode.BadRequest;
                    result = $"Argument '{argumentNullException.ParamName}' is required";
                    break;
            }
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            if(result == string.Empty)
            {
                result = JsonSerializer.Serialize(new {error = exception.Message});
            }

            return context.Response.WriteAsync(result);
        }
    }
}
