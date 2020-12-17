﻿using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Web.MVC.Helper
{
    /// <summary>
    /// 通過gateway調用服務
    /// </summary>
    public class GatewayServiceHelper : IServiceHelper
    {
        public async Task<string> GetOrder(string accessToken)
        {
            var Client = new RestClient("http://localhost:9070");
            var request = new RestRequest("/orders", Method.GET);
            request.AddHeader("Authorization", "Bearer " + accessToken);

            var response = await Client.ExecuteAsync(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return response.StatusCode + " " + response.Content;
            }
            return response.Content;
        }

        public async Task<string> GetProduct(string accessToken)
        {
            var Client = new RestClient("http://localhost:9070");
            var request = new RestRequest("/products", Method.GET);
            request.AddHeader("Authorization", "Bearer " + accessToken);

            var response = await Client.ExecuteAsync(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return response.StatusCode + " " + response.Content;
            }
            return response.Content;
        }

        public void GetServices()
        {
            throw new NotImplementedException();
        }
    }
}
