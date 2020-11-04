using Domain.Models;
using Domain.Services.Enums;
using Domain.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _dbContext;

        public EmployeeService(IUnitOfWork dbContext)
        {
            _dbContext = dbContext;
        }

        public Task AddNewEmployee(Employee employee)
        {
            _dbContext.Add(employee);
            _dbContext.SetEntityState(employee.Position, State.Unchanged);

            var now = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
            var newPositionHistoryEntry = new EmployeePositionHistory { StartDate = now, Position = employee.Position };
            employee.PositionHistory.Add(newPositionHistoryEntry);

            return _dbContext.SaveChangesAsync();
        }

        public async Task ArchiveEmployee(int id)
        {
            var employee = await _dbContext.Query<Employee>().Where(e => e.Id == id).FirstOrDefaultAsync();
            employee.IsArchived = true;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteEmployee(Employee employee)
        {
            _dbContext.Delete(employee);
            var affectedRows = await _dbContext.SaveChangesAsync();
            return affectedRows != 0;
        }

        public Task<Employee> GetEmployee(int id)
        {
            return _dbContext.Query<Employee>().Where(e => e.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyCollection<Employee>> GetArchivedEmployees()
        {
            var employees = await _dbContext.Query<Employee>().Where(e => e.IsArchived == true).ToListAsync();
            return employees;
        }

        public async Task<IReadOnlyCollection<Employee>> GetCurrentEmployees()
        {
            var employees = await _dbContext.Query<Employee>().Where(e => e.IsArchived == false).ToListAsync();
            return employees;
        }

        public async Task<bool> UpdateEmployee(Employee employee)
        {
            var dbEmployee = await _dbContext.Query<Employee>().Include(e => e.PositionHistory).FirstOrDefaultAsync(e => e.Id == employee.Id);
            if (dbEmployee == null) return false;

            _dbContext.SetEntityState(employee.Position, State.Unchanged);
            if (employee.Position.Id != dbEmployee.Position.Id)
                UpdatePositionHistory(dbEmployee, employee.Position);

            UpdateEmployeeProperties(dbEmployee, employee);

            var affectedRows = await _dbContext.SaveChangesAsync();
            return affectedRows != 0;
        }

        private void UpdatePositionHistory(Employee dbEmployee, Position newPosition)
        {
            var dbCurrentPosition = dbEmployee.PositionHistory.Single(ph => !ph.EndDate.HasValue);
            var now = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
            dbCurrentPosition.EndDate = now;
            var newPositionHistoryEntry = new EmployeePositionHistory { StartDate = now, Position = newPosition };
            dbEmployee.PositionHistory.Add(newPositionHistoryEntry);
        }

        private void UpdateEmployeeProperties(Employee dbEmployee, Employee dtoEmployee)
        {
            dbEmployee.Position = dtoEmployee.Position;
            dbEmployee.FirstName = dtoEmployee.FirstName;
            dbEmployee.Surname = dtoEmployee.Surname;
            dbEmployee.Birthday = dtoEmployee.Birthday;
            dbEmployee.StartDate = dtoEmployee.StartDate;
            dbEmployee.Salary = dtoEmployee.Salary;
            dbEmployee.Address = dtoEmployee.Address;
        }
    }
}
