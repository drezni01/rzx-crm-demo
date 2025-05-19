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
        private readonly SeedDataConfig _config;
        private readonly IDataRepository _dataRepository;
        private readonly ILogger _logger;

        public SeedingService(SeedDataConfig config, IDataRepository dataRepository, ILogger<SeedingService> logger)
        {
            _config = config;
            _dataRepository = dataRepository;
            _logger = logger;
        }

        public async Task SeedDataAsync()
        {
            if (_config.SeedType == SeedTypeEnum.WHEN_EMPTY)
            {
                var orders = (await _dataRepository.GetAllOrdersAsync()).ToList();
                if (orders.Any())
                {
                    _logger.LogInformation($"skip seeding as there are {orders.Count()} orders");
                    return;
                }
            }

            _logger.LogInformation("deleting exising data");
            await _dataRepository.DeleteAllOrdersAsync();
            await _dataRepository.DeleteAllCustomersAsync();
            await _dataRepository.DeleteAllProductsAsync();
            await _dataRepository.DeleteAllEmployeesAsync();

            await SeedCustomersAsync();
            await SeedProductsAsync();
            await SeedEmployeesAsync();

            var customers = (await _dataRepository.GetAllCustomersAsync()).ToList();
            var products = (await _dataRepository.GetAllProductsAsync()).ToList();
            var employees = (await _dataRepository.GetAllEmployeesAsync()).ToList();

            _logger.LogInformation($"seeding {_config.OrdersCount} orders");
            var newOrders = new List<Order>();
            for (int i = 0; i < _config.OrdersCount; i++)
            {
                var faker = new Faker();
                newOrders.Add(new Order
                {
                    CustomerId = faker.PickRandom(customers).CustomerId,
                    SalesPersonId = faker.PickRandom(employees).EmployeeId,
                    ProductId = faker.PickRandom(products).ProductId,
                    Quantity = faker.Random.Number(10, 10000),
                    Timestamp = DateTime.UtcNow
                });
            }

            foreach (var batch in newOrders.Batch(10_000))
            {
                await _dataRepository.AddOrdersAsync(batch);
            }
        }

        private async Task SeedEmployeesAsync()
        {
            _logger.LogInformation($"seeding {_config.EmployeesCount} employees");
            var employees = new List<Employee>();

            for (int i = 0; i < _config.EmployeesCount; i++)
            {
                var faker = new Faker();
                employees.Add(new Employee
                {
                    FirstName = faker.Person.FirstName,
                    LastName = faker.Person.LastName,
                    MiddleInitial = faker.Person.FirstName.Substring(0, 1),
                    Timestamp = DateTime.UtcNow
                });
            }

            await _dataRepository.AddEmployeesAsync(employees);
        }

        private async Task SeedCustomersAsync()
        {
            _logger.LogInformation($"seeding {_config.CustomersCount} Customers");
            var customers = new List<Customer>();

            for (int i = 0; i < _config.CustomersCount; i++)
            {
                var faker = new Faker();
                customers.Add(new Customer
                {
                    FirstName = faker.Person.FirstName,
                    LastName = faker.Person.LastName,
                    MiddleInitial = faker.Person.FirstName.Substring(0, 1),
                    Timestamp = DateTime.UtcNow
                });
            }

            await _dataRepository.AddCustomersAsync(customers);
        }

        private async Task SeedProductsAsync()
        {
            _logger.LogInformation($"seeding {_config.ProductsCount} Products");
            var products = new List<Product>();

            for (int i = 0; i < _config.ProductsCount; i++)
            {
                var faker = new Faker();
                products.Add(new Product
                {
                    Name = faker.Commerce.ProductName(),
                    Price = Convert.ToDouble(faker.Commerce.Price()),
                    Timestamp = DateTime.UtcNow
                });
            }

            await _dataRepository.AddProductsAsync(products);
        }
    }
}