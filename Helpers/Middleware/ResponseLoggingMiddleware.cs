namespace Api.Helper.Middleware
{
    public class ResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Business.Abstract.ILogger _logger;

        public ResponseLoggingMiddleware(RequestDelegate next, Business.Abstract.ILogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;
            using (var memoryStream = new MemoryStream())
            {
                context.Response.Body = memoryStream;

                await _next(context);


                memoryStream.Seek(0, SeekOrigin.Begin);
                var responseBody = new StreamReader(memoryStream).ReadToEnd();
                var logData = new
                {
                    StatusCode = context.Response.StatusCode,
                    ResponseBody = responseBody,
                    Timestamp = DateTime.UtcNow
                };

                _logger.LogInformation(nameof(ResponseLoggingMiddleware), logData);

                memoryStream.Seek(0, SeekOrigin.Begin);
                await memoryStream.CopyToAsync(originalBodyStream);
            }
        }
    }


}
