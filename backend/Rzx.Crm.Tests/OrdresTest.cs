using Microsoft.Extensions.DependencyInjection;
using Rzx.Crm.Core.Models;
using Rzx.Crm.Core.Services;
using Rzx.Crm.Core.Exceptions;

namespace Rzx.Crm.Tests
{
    [TestClass]
    public class OrdersTest : TestBase
    {
        [TestMethod]
        public async Task TestCrud()
        {
            var orderSvc = ServiceProvider.GetRequiredService<OrderService>();            
            var productSvc = ServiceProvider.GetRequiredService<ProductService>();
            var customerSvc = ServiceProvider.GetRequiredService<CustomerService>();
            var employeeSvc = ServiceProvider.GetRequiredService<EmployeeService>();

            var customer = new Customer
            {
                FirstName = "John",
                LastName = "Doe",
                MiddleInitial = "T"
            };
            await customerSvc.AddCustomerAsync(customer);

            var employee = new Employee
            {
                FirstName = "John",
                LastName = "Doe",
                MiddleInitial = "T"
            };
            await employeeSvc.AddEmployeeAsync(employee);

            var product = new Product
            {
                Name = "Test",
                Price = 100,
                Timestamp = DateTime.UtcNow
            };
            await productSvc.AddProductAsync(product);

            var order = new Order
            {
                CustomerId = customer.CustomerId,
                ProductId = product.ProductId,
                SalesPersonId = employee.EmployeeId,
                Quantity = 100
            };

            await orderSvc.AddOrderAsync(order);
            Assert.IsTrue((await orderSvc.GetAllOrdersAsync()).Any(), "did not insert");

            order.Quantity = 200;
            await orderSvc.UpdateOrderAsync(order);

            order= await orderSvc.GetOrderByIdAsync(order.OrderId);
            Assert.AreEqual(200, order.Quantity, "did not update");

            await orderSvc.DeleteOrderAsync(order.OrderId);
            Assert.IsNull(await orderSvc.GetOrderByIdAsync(order.OrderId), "did not delete");;
        }        
    }
}
