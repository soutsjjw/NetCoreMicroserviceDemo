using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Order.API.MessageDto;
using Order.API.Models;
using System;
using System.Threading.Tasks;

namespace Order.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<OrdersController> _logger;
        private readonly IConfiguration _configuration;
        private readonly ICapPublisher _capBus;
        private readonly OrderContext _context;

        public OrdersController(ILogger<OrdersController> logger, IConfiguration configuration, 
            ICapPublisher capPublisher, OrderContext context)
        {
            _logger = logger;
            _configuration = configuration;
            _capBus = capPublisher;
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            string result = $"【訂單服務】{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}——" + 
                $"{Request.HttpContext.Connection.LocalIpAddress}:{_configuration["ConsulSetting:ServicePort"]}";
            return Ok(result);
        }

        /// <summary>
        /// 下單 發佈下單事件
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        [Route("Create")]
        [HttpPost]
        public async Task<IActionResult> CreateOrder(Models.Order order)
        {
            using (var trans = _context.Database.BeginTransaction(_capBus, autoCommit: true))
            {
                // 業務代碼
                order.CreateTime = DateTime.Now;
                _context.Orders.Add(order);

                var r = await _context.SaveChangesAsync() > 0;

                if (r)
                {
                    // 發佈下單事件
                    await _capBus.PublishAsync("order.services.createorder", new CreateOrderMessageDto()
                    {
                        Count = order.Count,
                        ProductID = order.ProductID
                    });
                    return Ok();
                }
                return BadRequest();
            }
        }
    }
}
