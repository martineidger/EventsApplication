namespace EventsApp.Middlewares
{
    public static class CustomExceptionsHandlerMiddlewareExtension
    {
        public static IApplicationBuilder UseCustomExceptionsHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomExceptionsHandlerMiddleware>();
        }
    }
}
