using Microsoft.AspNetCore.Mvc;
using Rzx.Crm.Core.Models;
using Rzx.Crm.Core.Services;

namespace Rzx.Crm.Api.WebApi.Controllers
{
    [Route("customers")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly WebApiConfig _webApiConfig;
        private readonly CustomerService _customerService;

        public CustomerController(WebApiConfig webApiConfig, CustomerService customerService)
        {
            _webApiConfig = webApiConfig;
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<IEnumerable<Customer>> GetCustomers()
        {
            await Task.Delay(TimeSpan.FromSeconds(_webApiConfig.FakeDelaySec));
            return await _customerService.GetAllCustomersAsync();
        }

        [HttpPost]
        public async Task<Customer> AddCustomer(Customer customer)
        {
            await Task.Delay(TimeSpan.FromSeconds(_webApiConfig.FakeDelaySec));
            await _customerService.AddCustomerAsync(customer);
            return customer;
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<Customer> UpdateCustomer(int id, Customer customer)
        {
            await Task.Delay(TimeSpan.FromSeconds(_webApiConfig.FakeDelaySec));
            await _customerService.UpdateCustomerAsync(customer);
            return customer;
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            await Task.Delay(TimeSpan.FromSeconds(_webApiConfig.FakeDelaySec));
            await _customerService.DeleteCustomerAsync(id);
            return Ok(id);
        }
    }
}
