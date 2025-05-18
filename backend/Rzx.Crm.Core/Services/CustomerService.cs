using MediatR;
using Microsoft.Extensions.Logging;
using Rzx.Crm.Core.Interfaces;
using Rzx.Crm.Core.Models;
using Rzx.Crm.Core.Events;

namespace Rzx.Crm.Core.Services
{
    public class CustomerService
    {
        private readonly IDataRepository dataRepository;
        private readonly IMediator mediator;
        private readonly ILogger logger;

        public CustomerService(IDataRepository dataRepository, IMediator mediator, ILogger<CustomerService> logger)
        {
            this.dataRepository = dataRepository;
            this.mediator = mediator;
            this.logger = logger;
        }

        public Task<Customer> GetCustomerByIdAsync(int customerId)
        {
            return dataRepository.GetCustomerByIdAsync(customerId);
        }

        public Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return dataRepository.GetAllCustomersAsync();
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            await dataRepository.AddCustomerAsync(customer);
            await mediator.Publish(new EntityModificationNotification<Customer>(customer, EntityModificationTypeEnum.ADD));
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            await dataRepository.UpdateCustomerAsync(customer);
            await mediator.Publish(new EntityModificationNotification<Customer>(customer, EntityModificationTypeEnum.UPDATE));
        }

        public async Task DeleteCustomerAsync(int customerId)
        {
            var customer = await GetCustomerByIdAsync(customerId);
            await dataRepository.DeleteCustomerAsync(customerId);
            await mediator.Publish(new EntityModificationNotification<Customer>(customer, EntityModificationTypeEnum.DELETE));
        }

        public Task DeleteAllCustomersAsync()
        {
            return dataRepository.DeleteAllCustomersAsync();
        }

    }
}
