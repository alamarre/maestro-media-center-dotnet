using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using LruCacheNet;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace MaestroServer.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class UserMiddleware
    {
        internal static object USER_KEY = new { };
        private readonly RequestDelegate _next;
        private readonly IHttpClientFactory httpClientFactory;
        private HttpClient client;
        private static LruCache<string, string> cache = new LruCache<string, string>();

        public UserMiddleware(RequestDelegate next, IHttpClientFactory httpClientFactory)
        {
            _next = next;
            this.httpClientFactory = httpClientFactory;
        }

        private async Task<string> GetEmail(HttpContext httpContext)
        {
            var authHeader = httpContext?.Request?.Headers["Authorization"].FirstOrDefault();

            if(authHeader == null)
            {
                return null;
            }
            if (cache.TryGetValue(authHeader, out string email))
            {
                return email;
            }
            var route = httpContext?.User?.FindAll("aud").FirstOrDefault(a => a.Value.EndsWith("userinfo"));


            if (route?.Value != null && authHeader != null)
            {
                if (client == null)
                {
                    client = httpClientFactory.CreateClient();
                }

                using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, route.Value))
                {
                    requestMessage.Headers.Add("Authorization", authHeader);

                    var response = await client.SendAsync(requestMessage);
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(body);
                    if (values.TryGetValue("email", out object parsedEmail)
                        && values.TryGetValue("email_verified", out object verified)
                        && (bool)verified)
                    {
                        cache.Add(authHeader, parsedEmail.ToString());
                        return parsedEmail?.ToString();
                    }
                }
            }

            return null;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var email = await GetEmail(httpContext);
            if(email != null)
            {
                httpContext.Items.Add(USER_KEY, email);
            }
            await _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class UserMiddlewareExtensions
    {
        public static IApplicationBuilder UseUserProfiles(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UserMiddleware>();
        }
    }
}
