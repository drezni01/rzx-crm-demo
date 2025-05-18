using Microsoft.Extensions.DependencyInjection;
using Rzx.Crm.Core.Models;
using Rzx.Crm.Core.Services;
using Rzx.Crm.Core.Exceptions;

namespace Rzx.Crm.Tests
{
    [TestClass]
    public class CustomersTest : TestBase
    {
        [TestMethod]
        public async Task TestCrud()
        {
            var svc = ServiceProvider.GetRequiredService<CustomerService>();

            var customer = new Customer
            {
                FirstName = "John",
                LastName = "Doe",
                MiddleInitial = "T"
            };

            await svc.AddCustomerAsync(customer);
            Assert.IsTrue((await svc.GetAllCustomersAsync()).Any(), "did not insert");

            customer.MiddleInitial = "R";
            await svc.UpdateCustomerAsync(customer);

            customer = await svc.GetCustomerByIdAsync(customer.CustomerId);
            Assert.AreEqual("R", customer.MiddleInitial, "did not update");

            await svc.DeleteCustomerAsync(customer.CustomerId);
            Assert.IsNull(await svc.GetCustomerByIdAsync(customer.CustomerId), "did not delete");;
        }

        [TestMethod]
        public async Task TestConcurrency()
        {
            var svc = ServiceProvider.GetRequiredService<CustomerService>();

            var customer = new Customer
            {
                FirstName = "John",
                LastName = "Doe",
                MiddleInitial = "T"
            };

            await svc.AddCustomerAsync(customer);

            var customer2 = await svc.GetCustomerByIdAsync(customer.CustomerId);

            customer.MiddleInitial = "R";
            await svc.UpdateCustomerAsync(customer);

            customer2.MiddleInitial = "U";
            await Assert.ThrowsExceptionAsync<EntityStaleException>(() => svc.UpdateCustomerAsync(customer2));
        }
    }
}
