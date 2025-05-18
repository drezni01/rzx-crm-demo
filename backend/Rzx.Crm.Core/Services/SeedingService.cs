using Bogus;
using MoreLinq;
using Microsoft.Extensions.Logging;
using Rzx.Crm.Core.Configuration;
using Rzx.Crm.Core.Interfaces;
using Rzx.Crm.Core.Models;

namespace Rzx.Crm.Core.Services
{
    public class SeedingService
    {
        private readonly SeedDataConfig config;
        private readonly IDataRepository dataRepository;
        private readonly ILogger logger;

        public SeedingService(SeedDataConfig config, IDataRepository dataRepository, ILogger<SeedingService> logger)
        {
            this.config = config;
            this.dataRepository = dataRepository;
            this.logger = logger;
        }

        public async Task SeedDataAsync()
        {
            if(config.SeedType == SeedTypeEnum.WHEN_EMPTY)
            {
                var orders = (await dataRepository.GetAllOrdersAsync()).ToList();
                if(orders.Any())
                {
                    logger.LogInformation($"skip seeding as there are {orders.Count()} orders");
                    return;
                }
            }

            logger.LogInformation("deleting exising data");
            await dataRepository.DeleteAllOrdersAsync();
            await dataRepository.DeleteAllCustomersAsync();
            await dataRepository.DeleteAllProductsAsync();
            await dataRepository.DeleteAllEmployeesAsync();

            await SeedCustomersAsync();
            await SeedProductsAsync();
            await SeedEmployeesAsync();

            var customers = (await dataRepository.GetAllCustomersAsync()).ToList();
            var products = (await dataRepository.GetAllProductsAsync()).ToList();
            var employees = (await dataRepository.GetAllEmployeesAsync()).ToList();

            logger.LogInformation($"seeding {config.OrdersCount} orders");
            var newOrders = new List<Order>();
            for(int i=0;i<config.OrdersCount;i++)
            {
                var faker = new Faker();
                newOrders.Add(new Order
                {
                    CustomerId = faker.PickRandom(customers).CustomerId,
                    SalesPersonId = faker.PickRandom(employees).EmployeeId,
                    ProductId = faker.PickRandom(products).ProductId,
                    Quantity = faker.Random.Number(10,10000),
                    Timestamp = DateTime.UtcNow
                });
            }

            foreach (var batch in newOrders.Batch(10_000))
            {
                await dataRepository.AddOrdersAsync(batch);
            }            
        }

        private async Task SeedEmployeesAsync()
        {
            logger.LogInformation($"seeding {config.EmployeesCount} employees");

            for(int i=0; i<config.EmployeesCount; i++)
            {
                var faker = new Faker();
                await dataRepository.AddEmployeeAsync(new Employee
                {
                    FirstName = faker.Person.FirstName,
                    LastName = faker.Person.LastName,
                    MiddleInitial = faker.Person.FirstName.Substring(0, 1),
                    Timestamp = DateTime.UtcNow
                });
            }
        }

        private async Task SeedCustomersAsync()
        {
            logger.LogInformation($"seeding {config.CustomersCount} Customers");

            for (int i = 0; i < config.CustomersCount; i++)
            {
                var faker = new Faker();
                await dataRepository.AddCustomerAsync(new Customer
                {
                    FirstName = faker.Person.FirstName,
                    LastName = faker.Person.LastName,
                    MiddleInitial = faker.Person.FirstName.Substring(0, 1),
                    Timestamp = DateTime.UtcNow
                });
            }
        }

        private async Task SeedProductsAsync()
        {
            logger.LogInformation($"seeding {config.ProductsCount} Products");

            for (int i = 0; i < config.ProductsCount; i++)
            {
                var faker = new Faker();
                await dataRepository.AddProductAsync(new Product
                {
                    Name = faker.Commerce.ProductName(),
                    Price = Convert.ToDouble(faker.Commerce.Price()),
                    Timestamp = DateTime.UtcNow
                });
            }
        }
    }
}
