using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Rzx.Crm.Core.Interfaces;
using Rzx.Crm.Core.Models;
using Rzx.Crm.Infra.Configuration;

namespace Rzx.Crm.Infra.Database
{
    public abstract class DataRepositoryBase : IDataRepository
    {
        protected DatabaseConfig databaseConfig;

        public DataRepositoryBase() { }

        public DataRepositoryBase(DatabaseConfig databaseConfig)
        {
            this.databaseConfig = databaseConfig;
        }

        protected abstract DbContextBase GetCtx();

        #region Employee
        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            using var ctx = GetCtx();
            return await ctx.Employees.OrderBy(c => c.LastName).ToListAsync();
        }

        public async Task AddEmployeeAsync(Employee employee)
        {
            using var ctx = GetCtx();
            await ctx.Employees.AddAsync(employee);
            await ctx.SaveChangesAsync();
        }

        public async Task DeleteAllEmployeesAsync()
        {
            using var ctx = GetCtx();
            await ctx.Employees.ExecuteDeleteAsync();
        }
        #endregion

        #region Customer
        public Task<Customer> GetCustomerByIdAsync(int customerId)
        {
            using var ctx = GetCtx();
            return ctx.Customers.FirstOrDefaultAsync(c => c.CustomerId == customerId);
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            using var ctx = GetCtx();
            await ctx.Customers.AddAsync(customer);
            await ctx.SaveChangesAsync();
        }

        public async Task DeleteCustomerAsync(int customerId)
        {
            using var ctx = GetCtx();
            var customer = await ctx.Customers.FirstOrDefaultAsync(c => c.CustomerId == customerId);
            if (customer == null)
                return;

            await ctx.Orders.Where(o => o.CustomerId == customerId).ExecuteDeleteAsync();
            ctx.Customers.Remove(customer);
            await ctx.SaveChangesAsync();
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            using var ctx = GetCtx();
            return await ctx.Customers.OrderBy(c => c.LastName).ToListAsync();
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            using var ctx = GetCtx();
            ctx.Customers.Update(customer);
            await ctx.SaveChangesAsync();
        }

        public async Task DeleteAllCustomersAsync()
        {
            using var ctx = GetCtx();
            await ctx.Customers.ExecuteDeleteAsync();
        }
        #endregion

        #region Product
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            using var ctx = GetCtx();
            return await ctx.Products.OrderBy(c => c.Name).ToListAsync();
        }

        public async Task AddProductAsync(Product product)
        {
            using var ctx = GetCtx();
            await ctx.Products.AddAsync(product);
            await ctx.SaveChangesAsync();
        }

        public async Task DeleteAllProductsAsync()
        {
            using var ctx = GetCtx();
            await ctx.Products.ExecuteDeleteAsync();
        }
        #endregion

        #region Order
        public Task<Order> GetOrderByIdAsync(int orderId)
        {
            using var ctx = GetCtx();
            return ctx.Orders.FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        public async Task AddOrderAsync(Order order)
        {
            using var ctx = GetCtx();
            await ctx.Orders.AddAsync(order);
            await ctx.SaveChangesAsync();
        }

        public async Task AddOrdersAsync(IEnumerable<Order> orders)
        {
            using var ctx = GetCtx();
            await ctx.BulkInsertAsync(orders);
            await ctx.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(int orderId)
        {
            using var ctx = GetCtx();
            var order = await ctx.Orders.FirstOrDefaultAsync(o => o.OrderId == orderId);
            if (order == null)
                return;

            ctx.Orders.Remove(order);
            await ctx.SaveChangesAsync();
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            using var ctx = GetCtx();
            return await ctx.Orders.OrderBy(o => o.OrderId).ToListAsync();
        }

        public async Task UpdateOrderAsync(Order order)
        {
            using var ctx = GetCtx();
            ctx.Orders.Update(order);
            await ctx.SaveChangesAsync();
        }

        public async Task DeleteAllOrdersAsync()
        {
            using var ctx = GetCtx();
            await ctx.Orders.ExecuteDeleteAsync();
        }
        #endregion
    }
}
