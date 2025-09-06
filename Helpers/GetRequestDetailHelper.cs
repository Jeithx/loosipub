namespace API.Helpers
{
    public static class GetRequestDetailHelper
    {
        public static string GetClientIpAddress(HttpRequest request)
        {
            string ip = request.Headers["X-Forwarded-For"];

            if (string.IsNullOrEmpty(ip))
                ip = request.HttpContext.Connection.RemoteIpAddress?.ToString();

            return ip ?? "IP Bulunamadı";
        }

        public static string GetUserAgent(HttpRequest request)
        {
            return request.Headers["User-Agent"].ToString();
        }
    }
}
