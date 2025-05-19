using Microsoft.AspNetCore.Mvc;
using Rzx.Crm.Core.Models;
using Rzx.Crm.Core.Services;

namespace Rzx.Crm.Api.WebApi.Controllers
{
    [Route("products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly WebApiConfig _webApiConfig;
        private readonly ProductService _productService;

        public ProductController(WebApiConfig webApiConfig, ProductService productService)
        {
            _webApiConfig = webApiConfig;
            _productService = productService;
        }

        [HttpGet]
        public async Task<IEnumerable<Product>> GetProducts()
        {
            await Task.Delay(TimeSpan.FromSeconds(_webApiConfig.FakeDelaySec));
            return await _productService.GetAllProductsAsync();
        }
    }
}
