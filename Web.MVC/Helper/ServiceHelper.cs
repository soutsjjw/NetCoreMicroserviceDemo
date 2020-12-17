using Consul;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Web.MVC.Helper
{
    public class ServiceHelper : IServiceHelper
    {
        private readonly IConfiguration _configuration;
        private readonly ConsulClient _consulClient;
        private ConcurrentBag<string> _orderServiceUrls;
        private ConcurrentBag<string> _productServiceUrls;
        private string OrderService, ProductService;

        public ServiceHelper(IConfiguration configuration)
        {
            _configuration = configuration;
            _consulClient = new ConsulClient(c =>
            {
                // consul地址
                c.Address = new Uri(_configuration["ConsulSetting:ConsulAddress"]);
            });
        }

        public async Task<string> GetOrder(string accessToken)
        {
            if (_orderServiceUrls == null)
                return await Task.FromResult("【訂單服務】正在初始化服務列表…");

            // 每次隨機訪問一個服務實例
            var Client = new RestClient(_orderServiceUrls.ElementAt(RandomNumberGenerator.GetInt32(0, _orderServiceUrls.Count)));
            var request = new RestRequest("/orders", Method.GET);

            var response = await Client.ExecuteAsync(request);
            return response.Content;
        }

        public async Task<string> GetProduct(string accessToken)
        {
            if (_productServiceUrls == null)
                return await Task.FromResult("【產品服務】正在初始化服務列表…");

            var Client = new RestClient(_productServiceUrls.ElementAt(RandomNumberGenerator.GetInt32(0, _productServiceUrls.Count)));
            var request = new RestRequest("/products", Method.GET);

            var response = await Client.ExecuteAsync(request);
            return response.Content;
        }

        public void GetServices()
        {
            var serviceNames = new string[] { nameof(OrderService), nameof(ProductService) };
            Array.ForEach(serviceNames, p =>
            {
                Task.Run(() =>
                {
                    // WaitTime默認為5分鐘
                    var queryOptions = new QueryOptions { WaitTime = TimeSpan.FromMinutes(10) };
                    while(true)
                    {
                        GetServices(queryOptions, p);
                    }
                });
            });
        }

        private void GetServices(QueryOptions queryOptions, string serviceName)
        {
            var res = _consulClient.Health.Service(serviceName, null, true, queryOptions).Result;

            // 控制台列印一下獲取服務列表的響應時間等訊息
            Console.WriteLine($"{DateTime.Now}獲取{serviceName}: queryOptions.WaitIndex: {queryOptions.WaitIndex} LastIndex: {res.LastIndex}");

            // 版本號不一致，說明服務列表發生了變化
            if (queryOptions.WaitIndex != res.LastIndex)
            {
                queryOptions.WaitIndex = res.LastIndex;

                // 服務地址列表
                var serviceUrls = res.Response.Select(p => $"http://{p.Service.Address + ":" + p.Service.Port}").ToArray();

                if (serviceName == nameof(OrderService))
                    _orderServiceUrls = new ConcurrentBag<string>(serviceUrls);
                if (serviceName == nameof(ProductService))
                    _productServiceUrls = new ConcurrentBag<string>(serviceUrls);
            }
        }
    }
}
