using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Ocelot.Provider.Polly;

namespace Ocelot.APIGateway
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication("orderService", options =>
                {
                    options.Authority = "https://localhost:9080";
                    options.ApiName = "orderApi";
                    options.SupportedTokens = SupportedTokens.Both;
                    options.ApiSecret = "orderApi secret";
                    //options.RequireHttpsMetadata = false;
                })
                .AddIdentityServerAuthentication("productService", options =>
                {
                    options.Authority = "https://localhost:9080";
                    options.ApiName = "productApi";
                    options.SupportedTokens = SupportedTokens.Both;
                    options.ApiSecret = "productApi secret";
                    //options.RequireHttpsMetadata = false;
                });

            // 添加ocelot服務
            services.AddOcelot()
                // 添加consul支持
                .AddConsul()
                // 添加緩存
                .AddCacheManager(x =>
                {
                    x.WithDictionaryHandle();
                })
                // 添加Polly
                .AddPolly();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // 設置ocelot中間件
            app.UseOcelot().Wait();
        }
    }
}
