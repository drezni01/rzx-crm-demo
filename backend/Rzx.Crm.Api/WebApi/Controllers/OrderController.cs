using Microsoft.AspNetCore.Mvc;
using Rzx.Crm.Core.Models;
using Rzx.Crm.Core.Services;
using System.Runtime.CompilerServices;

namespace Rzx.Crm.Api.WebApi.Controllers
{
    [Route("orders")]
    public class OrderController : ControllerBase
    {
        private readonly WebApiConfig _webApiConfig;
        private readonly OrderService _orderService;

        public OrderController(WebApiConfig webApiConfig, OrderService orderService)
        {
            _webApiConfig = webApiConfig;
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IEnumerable<Order>> GetOrders()
        {
            await Task.Delay(TimeSpan.FromSeconds(_webApiConfig.FakeDelaySec));
            return await _orderService.GetAllOrdersAsync();
        }

        [HttpGet]
        [Route("customer/{customerId}")]
        public async Task<IEnumerable<Order>> GetOrdersByCustomer(int customerId)
        {
            await Task.Delay(TimeSpan.FromSeconds(_webApiConfig.FakeDelaySec));
            return await _orderService.GetOrdersByCustomerId(customerId);
        }

        [HttpPost]
        public async Task<Order> AddOrder([FromBody]Order order)
        {
            await Task.Delay(TimeSpan.FromSeconds(_webApiConfig.FakeDelaySec));
            await _orderService.AddOrderAsync(order);
            return order;
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<Order> UpdateOrder(int id,[FromBody] Order order)
        {
            await Task.Delay(TimeSpan.FromSeconds(_webApiConfig.FakeDelaySec));
            await _orderService.UpdateOrderAsync(order);
            return order;
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            await Task.Delay(TimeSpan.FromSeconds(_webApiConfig.FakeDelaySec));
            await _orderService.DeleteOrderAsync(id);
            return Ok(id);
        }
    }
}
