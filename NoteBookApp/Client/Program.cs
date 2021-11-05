using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NoteBookApp.Client.Services;
using Radzen;

namespace NoteBookApp.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddSingleton<IFileUploader, AzureBlobService>();
            builder.Services.AddScoped<DialogService>();
            builder.Services.AddScoped<NotificationService>();
            //builder.Services.AddHttpClient("NoteBookApp.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
            //    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            // Supply HttpClient instances that include access tokens when making requests to the server project
            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("NoteBookApp.ServerAPI"));

            builder.Services.AddApiAuthorization();
            var baseUrl = GetBaseUrl(builder.HostEnvironment);
            AddHttpClient<INoteDataService, NoteDataService>(builder.Services, baseUrl);
            AddHttpClient<IFileDataService, FileDataService>(builder.Services, baseUrl);
            AddHttpClient<IAccountDataService, AccountDataService>(builder.Services, baseUrl);
            
            await builder.Build().RunAsync().ConfigureAwait(false);
        }

        static string GetBaseUrl(IWebAssemblyHostEnvironment webAssemblyHostEnvironment)
        {
            var url = webAssemblyHostEnvironment.BaseAddress;
            return url;
        }

        private static void AddHttpClient<TInterface, TClass>(IServiceCollection services,
            string url) where TClass : class, TInterface where TInterface : class
        {
            services.AddHttpClient<TInterface, TClass>(client => client.BaseAddress = new Uri(url))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

        }
        //private static void AddHttpClient<TInterface, TClass>(IServiceCollection services,
        //    string url) where TClass : class, TInterface where TInterface : class
        //{
        //    services.AddHttpClient<TInterface, TClass>(client => client.BaseAddress = new Uri(url))
        //        .AddHttpMessageHandler(sp =>
        //        {
        //            var handler = sp.GetService<AuthorizationMessageHandler>()!
        //                .ConfigureHandler(authorizedUrls: new[] { url }, scopes: new[] { "rentier" });
        //            return handler;

        //        });
        //}
    }
}
