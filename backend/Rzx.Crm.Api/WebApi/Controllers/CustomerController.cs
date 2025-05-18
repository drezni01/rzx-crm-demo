using Microsoft.AspNetCore.Mvc;
using Rzx.Crm.Core.Models;
using Rzx.Crm.Core.Services;

namespace Rzx.Crm.Api.WebApi.Controllers
{
    [Route("customers")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private CustomerService customerService;

        public CustomerController(CustomerService customerService)
        {
            this.customerService = customerService;
        }

        [HttpGet]
        public async Task<IEnumerable<Customer>> GetCustomers()
        {
            await Task.Delay(1000);
            return await customerService.GetAllCustomersAsync();
        }

        [HttpPost]
        public async Task<Customer> AddCustomer(Customer customer)
        {
            await Task.Delay(1000);
            await customerService.AddCustomerAsync(customer);
            return customer;
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<Customer> UpdateCustomer(int id, Customer customer)
        {
            await Task.Delay(1000);
            await customerService.UpdateCustomerAsync(customer);
            return customer;
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            await Task.Delay(1000);
            await customerService.DeleteCustomerAsync(id);
            return Ok(id);
        }
    }
}
