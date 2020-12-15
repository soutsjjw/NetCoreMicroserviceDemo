using RestSharp;
using System.Threading.Tasks;

namespace Web.MVC.Helper
{
    public class ServiceHelper : IServiceHelper
    {
        public async Task<string> GetOrder()
        {
            string serviceUrl = "http://localhost:9060";    // 訂單服務的地址，可以放在配置文件或者資料庫等等…

            var Client = new RestClient(serviceUrl);
            var request = new RestRequest("/orders", Method.GET);

            var response = await Client.ExecuteAsync(request);
            return response.Content;
        }

        public async Task<string> GetProduct()
        {
            string serviceUrl = "http://localhost:9050";    // 產品服務的地址，可以放在配置文件或者資料庫等等…

            var Client = new RestClient(serviceUrl);
            var request = new RestRequest("/products", Method.GET);

            var response = await Client.ExecuteAsync(request);
            return response.Content;
        }
    }
}
