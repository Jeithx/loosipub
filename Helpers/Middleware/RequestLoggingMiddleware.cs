namespace Api.Helper.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Business.Abstract.ILogger _logger;

        public RequestLoggingMiddleware(RequestDelegate next, Business.Abstract.ILogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var request = context.Request;
            var ipAddress = context.Connection.RemoteIpAddress?.ToString();
            var userName = context.User.Identity?.Name ?? "Anonymous";

            var logData = new
            {
                IpAddress = ipAddress,
                UserName = userName,
                request.Path,
                request.Method,
                QueryString = request.QueryString.ToString(),
                Timestamp = DateTime.UtcNow
            };

            _logger.LogInformation(nameof(RequestLoggingMiddleware), logData);

            await _next(context);
        }
    }

}
