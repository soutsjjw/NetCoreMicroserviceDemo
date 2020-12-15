using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace Order.API.Helper
{
    public static class ConsulHelper
    {
        public static IApplicationBuilder RegisterConsul(this IApplicationBuilder app, 
            IConfiguration configuration, IHostApplicationLifetime lifetime)
        {
            var consulClient = new ConsulClient(c =>
            {
                // consul地址
                c.Address = new Uri(configuration["ConsulSetting:ConsulAddress"]);
            });

            var registration = new AgentServiceRegistration()
            {
                // 服務實例唯一標識
                ID = Guid.NewGuid().ToString(),
                // 服務名稱
                Name = configuration["ConsulSetting:ServiceName"],
                // 服務IP
                Address = configuration["ConsulSetting:ServiceIP"],
                // 服務端口，因為要運行多個實例，端口不能在appsettings.json裡配置，在docker容器運行時傳入
                Port = int.Parse(configuration["ConsulSetting:ServicePort"]),
                Check = new AgentServiceCheck()
                {
                    // 服務啟動多久後註冊
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),
                    // 健康檢查時間間隔
                    Interval = TimeSpan.FromSeconds(10),
                    // 健康檢查地址
                    HTTP = $"http://{configuration["ConsulSetting:ServiceIP"]}:{configuration["ConsulSetting:ServicePort"]}{configuration["ConsulSetting:ServiceHealthCheck"]}",
                    // 超時時間
                    Timeout = TimeSpan.FromSeconds(5)
                }
            };

            // 服務註冊
            consulClient.Agent.ServiceRegister(registration).Wait();

            // 應用程序終止時，取消註冊
            lifetime.ApplicationStopping.Register(() =>
            {
                consulClient.Agent.ServiceDeregister(registration.ID).Wait();
            });

            return app;
        }
    }
}
