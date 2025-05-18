using Microsoft.Extensions.DependencyInjection;
using Rzx.Crm.Core.Models;
using Rzx.Crm.Core.Services;

namespace Rzx.Crm.Tests
{
    [TestClass]
    public class EmployeeTest : TestBase
    {
        [TestMethod]
        public async Task TestCrud()
        {
            var svc = ServiceProvider.GetRequiredService<EmployeeService>();

            var employee = new Employee
            {
                FirstName = "John",
                LastName = "Doe",
                MiddleInitial = "T"
            };

            await svc.AddEmployeeAsync(employee);
            Assert.IsTrue((await svc.GetAllEmployeesAsync()).Any(), "did ont insert");
        }
    }
}
