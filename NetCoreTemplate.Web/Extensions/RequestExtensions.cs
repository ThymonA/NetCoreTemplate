namespace NetCoreTemplate.Web.Extensions
{
    using System;

    using Microsoft.AspNetCore.Http;

    public static class RequestExtensions
    {
        public static string GetIPAddress(this HttpRequest request)
        {
            if (request.Headers.ContainsKey("X-Forwarded-For"))
            {
                var requests = request.Headers["X-Forwarded-For"].ToString()
                    .Split(", ", StringSplitOptions.RemoveEmptyEntries);

                if (requests.Length < 1)
                {
                    return request.HttpContext?.Connection?.RemoteIpAddress?.MapToIPv4().ToString();
                }

                return requests[0];
            }

            return request.HttpContext?.Connection?.RemoteIpAddress?.MapToIPv4().ToString();
        }
    }
}
