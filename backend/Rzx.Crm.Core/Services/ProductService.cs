using Microsoft.Extensions.Logging;
using Rzx.Crm.Core.Interfaces;
using Rzx.Crm.Core.Models;

namespace Rzx.Crm.Core.Services
{
    public class ProductService
    {
        private readonly IDataRepository dataRepository;
        private readonly ILogger logger;

        public ProductService(IDataRepository dataRepository, ILogger<ProductService> logger)
        {
            this.dataRepository = dataRepository;
            this.logger = logger;
        }

        public Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return dataRepository.GetAllProductsAsync();
        }

        public Task AddProductAsync(Product Product)
        {
            return dataRepository.AddProductAsync(Product);
        }

        public Task DeleteAllProductsAsync()
        {
            return dataRepository.DeleteAllProductsAsync();
        }

    }
}
