using System.Threading.Tasks;

namespace Web.MVC.Helper
{
    public interface IServiceHelper
    {
        /// <summary>
        /// 獲取產品資料
        /// </summary>
        /// <returns></returns>
        Task<string> GetProduct(string accessToken);

        /// <summary>
        /// 獲取訂單資料
        /// </summary>
        /// <returns></returns>
        Task<string> GetOrder(string accessToken);

        /// <summary>
        /// 獲取服務列表
        /// </summary>
        void GetServices();
    }
}
