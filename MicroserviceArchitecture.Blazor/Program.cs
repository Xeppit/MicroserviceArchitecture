using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MicroserviceArchitecture.Blazor
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddOidcAuthentication(options =>
            {
                builder.Configuration.Bind("oidc", options.ProviderOptions);
                options.UserOptions.RoleClaim = "role";
            });

            builder.Services.AddHttpClient("api")                               // New Line
                .AddHttpMessageHandler(sp =>                                    // New Line
                {                                                               // New Line
                    var handler = sp.GetService<AuthorizationMessageHandler>()  // New Line
                        .ConfigureHandler(                                      // New Line
                            authorizedUrls: new[] { "https://localhost:5004" }, // New Line
                            scopes: new[] { "testapi" });                       // New Line
                    return handler;                                             // New Line
                });                                                             // New Line

            builder.Services.AddScoped(                                         // New Line
                sp => sp.GetService<IHttpClientFactory>().CreateClient("api")); // New Line

            await builder.Build().RunAsync();
        }
    }
}
