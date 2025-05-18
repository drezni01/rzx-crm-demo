using MediatR;
using Microsoft.Extensions.Logging;
using Rzx.Crm.Core.Events;
using Rzx.Crm.Core.Interfaces;
using Rzx.Crm.Core.Models;

namespace Rzx.Crm.Core.Services
{
    public class OrderService
    {
        private readonly IDataRepository dataRepository;
        private readonly IMediator mediator;
        private readonly ILogger logger;

        public OrderService(IDataRepository dataRepository, IMediator mediator, ILogger<OrderService> logger)
        {
            this.dataRepository = dataRepository;
            this.mediator = mediator;
            this.logger = logger;
        }

        public Task<Order> GetOrderByIdAsync(int orderId)
        {
            return dataRepository.GetOrderByIdAsync(orderId);
        }

        public Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return dataRepository.GetAllOrdersAsync();            
        }

        public async Task<IEnumerable<Order>> GetOrdersByCustomerId(int customerId)
        {
            var orders = await dataRepository.GetAllOrdersAsync();
            return orders.Where(o => o.CustomerId == customerId);
        }

        public async Task AddOrderAsync(Order order)
        {
            await dataRepository.AddOrderAsync(order);
            await mediator.Publish(new EntityModificationNotification<Order>(order, EntityModificationTypeEnum.ADD));
        }

        public async Task UpdateOrderAsync(Order order)
        {
            await dataRepository.UpdateOrderAsync(order);
            await mediator.Publish(new EntityModificationNotification<Order>(order, EntityModificationTypeEnum.UPDATE));
        }

        public async Task DeleteOrderAsync(int orderId)
        {
            var order = await GetOrderByIdAsync(orderId);
            await dataRepository.DeleteOrderAsync(orderId);
            await mediator.Publish(new EntityModificationNotification<Order>(order, EntityModificationTypeEnum.DELETE));
        }

        public Task DeleteAllOrdersAsync()
        {
            return dataRepository.DeleteAllOrdersAsync();
        }

    }
}
