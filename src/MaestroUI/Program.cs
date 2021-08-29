using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MaestroUI.interfaces;
using MaestroMediaCenter.Common;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace MaestroUI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            //builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddOidcAuthentication(options =>
            {
                builder.Configuration.Bind("Auth0", options.ProviderOptions);
                options.ProviderOptions.ResponseType = "code";
                options.ProviderOptions.DefaultScopes.Add("email");
                options.ProviderOptions.DefaultScopes.Add("test_scope");
            });
            builder.Services.AddSingleton<ISample, Sample>();

            builder.Services.AddScoped<CustomAuthorizationMessageHandler>();
            builder.Services.AddHttpClient("ServerAPI",
            client => client.BaseAddress = new Uri("https://localhost:48607/"))
              .AddHttpMessageHandler<CustomAuthorizationMessageHandler>();

            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
              .CreateClient("ServerAPI"));

            new Loader().LoadDefaults(builder.Services);
            await builder.Build().RunAsync();
        }
    }
}
