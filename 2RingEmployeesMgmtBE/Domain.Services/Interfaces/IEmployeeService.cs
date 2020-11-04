using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task AddNewEmployee(Employee employee);
        Task ArchiveEmployee(int id);
        Task<bool> DeleteEmployee(Employee employee);
        Task<IReadOnlyCollection<Employee>> GetArchivedEmployees();
        Task<IReadOnlyCollection<Employee>> GetCurrentEmployees();
        Task<Employee> GetEmployee(int id);
        Task<bool> UpdateEmployee(Employee employee);
    }
}
