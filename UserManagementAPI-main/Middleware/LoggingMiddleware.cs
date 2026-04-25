namespace UserManagementAPI.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            _logger.LogInformation(
                "[{Time}] {Method} {Path}",
                DateTime.UtcNow,
                context.Request.Method,
                context.Request.Path
            );

            await _next(context);

            _logger.LogInformation(
                "[{Time}] Response: {StatusCode}",
                DateTime.UtcNow,
                context.Response.StatusCode
            );
        }
    }
}
