using Microsoft.Extensions.DependencyInjection;
using Rzx.Crm.Core.Configuration;
using Rzx.Crm.Core.Services;

namespace Rzx.Crm.Tests
{
    [TestClass]
    public class SeedingTest : TestBase
    {
        [TestMethod]
        public async Task TestSeed()
        {
            var seedingCfg = ServiceProvider.GetRequiredService<SeedDataConfig>();
            var seedingSvc = ServiceProvider.GetRequiredService<SeedingService>();
            var customerSvc = ServiceProvider.GetRequiredService<CustomerService>();
            var employeeSvc = ServiceProvider.GetRequiredService<EmployeeService>();
            var productSvc = ServiceProvider.GetRequiredService<ProductService>();
            var orderSvc = ServiceProvider.GetRequiredService<OrderService>();

            var customers = await customerSvc.GetAllCustomersAsync();
            Assert.IsTrue(!customers.Any(), "residual entities");

            var employees = await employeeSvc.GetAllEmployeesAsync();
            Assert.IsTrue(!employees.Any(), "residual entities");

            var products = await productSvc.GetAllProductsAsync();
            Assert.IsTrue(!products.Any(), "residual entities");

            var orders = await orderSvc.GetAllOrdersAsync();
            Assert.IsTrue(!orders.Any(), "residual entities");

            await seedingSvc.SeedDataAsync();

            customers = await customerSvc.GetAllCustomersAsync();
            Assert.AreEqual(seedingCfg.CustomersCount, customers.Count(), "did not properly populate");

            employees = await employeeSvc.GetAllEmployeesAsync();
            Assert.AreEqual(seedingCfg.EmployeesCount, employees.Count(), "did not properly populate");

            products = await productSvc.GetAllProductsAsync();
            Assert.AreEqual(seedingCfg.ProductsCount, products.Count(), "did not properly populate");

            orders = await orderSvc.GetAllOrdersAsync();
            Assert.AreEqual(seedingCfg.OrdersCount, orders.Count(), "did not properly populate");
        }
    }
}
