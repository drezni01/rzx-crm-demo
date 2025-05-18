using Microsoft.Extensions.Logging;
using Rzx.Crm.Core.Interfaces;
using Rzx.Crm.Core.Models;

namespace Rzx.Crm.Core.Services
{
    public class EmployeeService
    {
        private readonly IDataRepository dataRepository;
        private readonly ILogger logger;

        public EmployeeService(IDataRepository dataRepository, ILogger<EmployeeService> logger)
        {
            this.dataRepository = dataRepository;
            this.logger = logger;
        }

        public Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return dataRepository.GetAllEmployeesAsync();
        }

        public Task AddEmployeeAsync(Employee employee)
        {
            return dataRepository.AddEmployeeAsync(employee);
        }

        public Task DeleteAllEmployeesAsync()
        {
            return dataRepository.DeleteAllEmployeesAsync();
        }

    }
}
