
using Rzx.Crm.Core.Models;

namespace Rzx.Crm.Core.Interfaces
{
    public interface IDataRepository
    {
        #region Employee
        Task<IEnumerable<Employee>> GetAllEmployeesAsync();
        Task AddEmployeeAsync(Employee employee);
        Task DeleteAllEmployeesAsync();
        #endregion

        #region Customer
        Task<Customer> GetCustomerByIdAsync(int customerId);
        Task<IEnumerable<Customer>> GetAllCustomersAsync();
        Task AddCustomerAsync(Customer customer);
        Task UpdateCustomerAsync(Customer customer);
        Task DeleteCustomerAsync(int customerId);
        Task DeleteAllCustomersAsync();
        #endregion

        #region Product
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task AddProductAsync(Product product);
        Task DeleteAllProductsAsync();
        #endregion

        #region Order
        Task<Order> GetOrderByIdAsync(int orderId);
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task AddOrderAsync(Order order);
        Task AddOrdersAsync(IEnumerable<Order> orders);
        Task UpdateOrderAsync(Order order);
        Task DeleteOrderAsync(int orderId);
        Task DeleteAllOrdersAsync();
        #endregion
    }
}
