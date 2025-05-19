using MediatR;
using Microsoft.Extensions.Logging;
using Ardalis.GuardClauses;
using Rzx.Crm.Core.Interfaces;
using Rzx.Crm.Core.Models;
using Rzx.Crm.Core.Events;

namespace Rzx.Crm.Core.Services
{
    public class CustomerService
    {
        private readonly IDataRepository _dataRepository;
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        public CustomerService(IDataRepository dataRepository, IMediator mediator, ILogger<CustomerService> logger)
        {
            _dataRepository = dataRepository;
            _mediator = mediator;
            _logger = logger;
        }

        public Task<Customer> GetCustomerByIdAsync(int customerId)
        {
            return _dataRepository.GetCustomerByIdAsync(customerId);
        }

        public Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return _dataRepository.GetAllCustomersAsync();
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            Validate(customer);
            await _dataRepository.AddCustomerAsync(customer);
            await _mediator.Publish(new EntityModificationNotification<Customer>(customer, EntityModificationTypeEnum.ADD));
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            Validate(customer);
            await _dataRepository.UpdateCustomerAsync(customer);
            await _mediator.Publish(new EntityModificationNotification<Customer>(customer, EntityModificationTypeEnum.UPDATE));
        }

        public async Task DeleteCustomerAsync(int customerId)
        {
            var customer = await GetCustomerByIdAsync(customerId);
            await _dataRepository.DeleteCustomerAsync(customerId);
            await _mediator.Publish(new EntityModificationNotification<Customer>(customer, EntityModificationTypeEnum.DELETE));
        }

        public Task DeleteAllCustomersAsync()
        {
            return _dataRepository.DeleteAllCustomersAsync();
        }

        private void Validate(Customer customer)
        {
            Guard.Against.NullOrEmpty(customer.FirstName, nameof(customer.FirstName));
            Guard.Against.NullOrEmpty(customer.LastName, nameof(customer.LastName));
        }
    }
}
