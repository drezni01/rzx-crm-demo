using Microsoft.Extensions.Logging;
using Rzx.Crm.Core.Interfaces;
using Rzx.Crm.Core.Models;

namespace Rzx.Crm.Core.Services
{
    public class EmployeeService
    {
        private readonly IDataRepository _dataRepository;
        private readonly ILogger _logger;

        public EmployeeService(IDataRepository dataRepository, ILogger<EmployeeService> logger)
        {
            _dataRepository = dataRepository;
            _logger = logger;
        }

        public Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return _dataRepository.GetAllEmployeesAsync();
        }

        public Task AddEmployeeAsync(Employee employee)
        {
            return _dataRepository.AddEmployeeAsync(employee);
        }

        public Task DeleteAllEmployeesAsync()
        {
            return _dataRepository.DeleteAllEmployeesAsync();
        }

    }
}
