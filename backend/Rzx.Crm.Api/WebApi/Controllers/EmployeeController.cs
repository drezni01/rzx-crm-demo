using Microsoft.AspNetCore.Mvc;
using Rzx.Crm.Core.Models;
using Rzx.Crm.Core.Services;

namespace Rzx.Crm.Api.WebApi.Controllers
{
    [Route("employees")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private EmployeeService employeeService;

        public EmployeeController(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        [HttpGet]
        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            return await employeeService.GetAllEmployeesAsync();
        }
    }
}
