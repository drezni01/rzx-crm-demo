using Microsoft.Extensions.Logging;
using Rzx.Crm.Core.Interfaces;
using Rzx.Crm.Core.Models;

namespace Rzx.Crm.Core.Services
{
    public class ProductService
    {
        private readonly IDataRepository _dataRepository;
        private readonly ILogger _logger;

        public ProductService(IDataRepository dataRepository, ILogger<ProductService> logger)
        {
            _dataRepository = dataRepository;
            _logger = logger;
        }

        public Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return _dataRepository.GetAllProductsAsync();
        }

        public Task AddProductAsync(Product Product)
        {
            return _dataRepository.AddProductAsync(Product);
        }

        public Task DeleteAllProductsAsync()
        {
            return _dataRepository.DeleteAllProductsAsync();
        }

    }
}
