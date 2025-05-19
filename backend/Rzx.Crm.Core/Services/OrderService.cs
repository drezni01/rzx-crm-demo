using Ardalis.GuardClauses;
using MediatR;
using Microsoft.Extensions.Logging;
using Rzx.Crm.Core.Events;
using Rzx.Crm.Core.Interfaces;
using Rzx.Crm.Core.Models;

namespace Rzx.Crm.Core.Services
{
    public class OrderService
    {
        private readonly IDataRepository _dataRepository;
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        public OrderService(IDataRepository dataRepository, IMediator mediator, ILogger<OrderService> logger)
        {
            _dataRepository = dataRepository;
            _mediator = mediator;
            _logger = logger;
        }

        public Task<Order> GetOrderByIdAsync(int orderId)
        {
            return _dataRepository.GetOrderByIdAsync(orderId);
        }

        public Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return _dataRepository.GetAllOrdersAsync();            
        }

        public Task<IEnumerable<Order>> GetOrdersByCustomerId(int customerId)
        {
            return _dataRepository.GetOrdersByCustomerAsync(customerId);
        }

        public async Task AddOrderAsync(Order order)
        {
            Validate(order);
            await _dataRepository.AddOrderAsync(order);
            await _mediator.Publish(new EntityModificationNotification<Order>(order, EntityModificationTypeEnum.ADD));
        }

        public async Task UpdateOrderAsync(Order order)
        {
            Validate(order);
            await _dataRepository.UpdateOrderAsync(order);
            await _mediator.Publish(new EntityModificationNotification<Order>(order, EntityModificationTypeEnum.UPDATE));
        }

        public async Task DeleteOrderAsync(int orderId)
        {
            var order = await GetOrderByIdAsync(orderId);
            await _dataRepository.DeleteOrderAsync(orderId);
            await _mediator.Publish(new EntityModificationNotification<Order>(order, EntityModificationTypeEnum.DELETE));
        }

        public Task DeleteAllOrdersAsync()
        {
            return _dataRepository.DeleteAllOrdersAsync();
        }

        private void Validate(Order order)
        {
            Guard.Against.NegativeOrZero(order.SalesPersonId, nameof(order.SalesPersonId));
            Guard.Against.NegativeOrZero(order.CustomerId, nameof(order.CustomerId));
            Guard.Against.NegativeOrZero(order.ProductId, nameof(order.ProductId));
            Guard.Against.NegativeOrZero(order.Quantity, nameof(order.Quantity));
        }
    }
}
