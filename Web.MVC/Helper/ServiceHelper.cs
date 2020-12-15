using Consul;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Web.MVC.Helper
{
    public class ServiceHelper : IServiceHelper
    {
        private readonly IConfiguration _configuration;

        public ServiceHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> GetOrder()
        {
            var consulClient = new ConsulClient(c =>
            {
                // consul地址
                c.Address = new Uri(_configuration["ConsulSetting:ConsulAddress"]);
            });

            // 健康的服務
            var services = consulClient.Health.Service("OrderService", null, true, null).Result.Response;

            // 訂單服務地址列表
            string[] serviceUrls = services.Select(p => $"http://{p.Service.Address + ":" + p.Service.Port}").ToArray();

            if (!serviceUrls.Any())
            {
                return await Task.FromResult("【訂單服務】服務列表為空");
            }

            // 每次隨機訪問一個服務實例
            var Client = new RestClient(serviceUrls[RandomNumberGenerator.GetInt32(0, serviceUrls.Length)]);
            var request = new RestRequest("/orders", Method.GET);

            var response = await Client.ExecuteAsync(request);
            return response.Content;
        }

        public async Task<string> GetProduct()
        {
            var consulClient = new ConsulClient(c =>
            {
                // consul地址
                c.Address = new Uri(_configuration["ConsulSetting:ConsulAddress"]);
            });

            // 健康的服務
            var services = consulClient.Health.Service("ProductService", null, true, null).Result.Response;

            // 產品服務地址列表
            string[] serviceUrls = services.Select(p => $"http://{p.Service.Address + ":" + p.Service.Port}").ToArray();

            if (!serviceUrls.Any())
            {
                return await Task.FromResult("【產品服務】服務列表為空");
            }

            var Client = new RestClient(serviceUrls[RandomNumberGenerator.GetInt32(0, serviceUrls.Length)]);
            var request = new RestRequest("/products", Method.GET);

            var response = await Client.ExecuteAsync(request);
            return response.Content;
        }
    }
}
