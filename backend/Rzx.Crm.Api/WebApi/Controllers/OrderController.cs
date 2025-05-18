using Microsoft.AspNetCore.Mvc;
using Rzx.Crm.Core.Models;
using Rzx.Crm.Core.Services;
using System.Runtime.CompilerServices;

namespace Rzx.Crm.Api.WebApi.Controllers
{
    [Route("orders")]
    public class OrderController : ControllerBase
    {
        private OrderService orderService;

        public OrderController(OrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpGet]
        public async Task<IEnumerable<Order>> GetOrders()
        {
            await Task.Delay(1000);
            return await orderService.GetAllOrdersAsync();
        }

        [HttpGet]
        [Route("customer/{customerId}")]
        public async Task<IEnumerable<Order>> GetOrdersByCustomer(int customerId)
        {
            await Task.Delay(1000);
            return await orderService.GetOrdersByCustomerId(customerId);
        }

        [HttpPost]
        public async Task<Order> AddOrder([FromBody]Order order)
        {
            await Task.Delay(1000);
            await orderService.AddOrderAsync(order);
            return order;
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<Order> UpdateOrder(int id,[FromBody] Order order)
        {
            await Task.Delay(1000);
            await orderService.UpdateOrderAsync(order);
            return order;
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            await Task.Delay(1000);
            await orderService.DeleteOrderAsync(id);
            return Ok(id);
        }
    }
}
