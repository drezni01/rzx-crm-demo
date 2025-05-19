using Microsoft.AspNetCore.Mvc;
using Rzx.Crm.Core.Models;
using Rzx.Crm.Core.Services;

namespace Rzx.Crm.Api.WebApi.Controllers
{
    [Route("employees")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly WebApiConfig _webApiConfig;
        private readonly EmployeeService _employeeService;

        public EmployeeController(WebApiConfig webApiConfig, EmployeeService employeeService)
        {
            _webApiConfig = webApiConfig;
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            await Task.Delay(TimeSpan.FromSeconds(_webApiConfig.FakeDelaySec));
            return await _employeeService.GetAllEmployeesAsync();
        }
    }
}
