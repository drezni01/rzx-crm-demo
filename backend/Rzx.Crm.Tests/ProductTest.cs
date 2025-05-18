using Microsoft.Extensions.DependencyInjection;
using Rzx.Crm.Core.Models;
using Rzx.Crm.Core.Services;

namespace Rzx.Crm.Tests
{
    [TestClass]
    public class ProductTest : TestBase
    {
        [TestMethod]
        public async Task TestCrud()
        {
            var svc = ServiceProvider.GetRequiredService<ProductService>();

            var product = new Product
            {
                Name = "Test",
                Price = 100,
                Timestamp = DateTime.UtcNow
            };

            await svc.AddProductAsync(product);
            Assert.IsTrue((await svc.GetAllProductsAsync()).Any(), "did ont insert");
        }
    }
}