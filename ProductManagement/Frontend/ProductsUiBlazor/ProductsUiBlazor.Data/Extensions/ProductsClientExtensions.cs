using Microsoft.Extensions.DependencyInjection;
using Microsoft.Kiota.Http.HttpClientLibrary;
using ProductManagement.ProductsApiClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Kiota.Abstractions.Authentication;

namespace ProductsUiBlazor.Data.Extensions;
public static class ProductsClientExtensions
{
    public static void AddProductsClient(this IHostApplicationBuilder builder)
    {
        builder.Services.AddServiceDiscovery();
        builder.Services.ConfigureHttpClientDefaults(http =>
        {
            http.AddServiceDiscovery();
        });
        //builder.Services.AddHttpClient().AddServiceDiscovery();

        builder.Services.AddScoped<IAuthenticationProvider>(sp =>
        {
            var hostEnvironment = sp.GetRequiredService<IHostEnvironment>(); // IHostEnvironment hier injizieren

            if (hostEnvironment.IsDevelopment())
            {
                return new AnonymousAuthenticationProvider();
            }
            var accessTokenProvider = sp.GetRequiredService<IAccessTokenProvider>();
            return new BaseBearerTokenAuthenticationProvider(accessTokenProvider);
        });

        builder.Services.AddScoped(sp =>
        {
            var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
            var client = httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://products-api");
            var hostEnvironment = sp.GetRequiredService<IHostEnvironment>();
            var authenticationProvider = sp.GetRequiredService<IAuthenticationProvider>();
            var adapter = new HttpClientRequestAdapter(authenticationProvider, httpClient: client);
            return new ProductsClient(adapter);
        });
    }
}
