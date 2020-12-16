using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.MVC.Helper
{
    /// <summary>
    /// 通過gateway調用服務
    /// </summary>
    public class GatewayServiceHelper : IServiceHelper
    {
        public async Task<string> GetOrder()
        {
            var client = new RestClient("http://localhost:9070");
            var request = new RestRequest("/orders", Method.GET);

            var response = await client.ExecuteAsync(request);
            return response.Content;
        }

        public async Task<string> GetProduct()
        {
            var client = new RestClient("http://localhost:9070");
            var request = new RestRequest("/products", Method.GET);

            var response = await client.ExecuteAsync(request);
            return response.Content;
        }

        public void GetServices()
        {
            throw new NotImplementedException();
        }
    }
}
