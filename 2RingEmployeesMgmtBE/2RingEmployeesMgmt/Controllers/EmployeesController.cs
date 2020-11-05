using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace _2RingEmployeesMgmt.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IValidator<Employee> _validator;

        public EmployeesController(IEmployeeService employeeService, IValidator<Employee> validator)
        {
            _employeeService = employeeService;
            _validator = validator;
        }

        [HttpGet]
        public async Task<IEnumerable<Employee>> GetCurrentEmployees()
        {
            var currentEmployees = await _employeeService.GetCurrentEmployees();
            return currentEmployees;
        }

        [HttpGet("archived")]
        public async Task<IEnumerable<Employee>> GetArchivedEmployees()
        {
            var dismissedEmployees = await _employeeService.GetArchivedEmployees();
            return dismissedEmployees;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var employee = await _employeeService.GetEmployee(id);
            if (employee == null)
                return NotFound();

            return employee;
        }

        [HttpPost]
        public async Task<ActionResult<Employee>> AddNewEmployee([FromBody] Employee employee)
        {
            var result = _validator.Validate(employee);
            if (!result.IsValid)
                return BadRequest(result.ToString());
            await _employeeService.AddNewEmployee(employee);

            return Created($"/Employees/{employee.Id}", employee);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] Employee employee)
        {
            employee.Id = id;
            var result = _validator.Validate(employee);
            if (!result.IsValid)
                return BadRequest(result);

            var isSuccess = await _employeeService.UpdateEmployee(employee);
            if (!isSuccess)
                return NotFound();
            return Ok();
        }

        [HttpPut("archive/{id}")]
        public async Task<IActionResult> ArchiveEmployee(int id)
        {
            var employee = await _employeeService.GetEmployee(id);
            if (employee == null)
                return NotFound();

            await _employeeService.ArchiveEmployee(id);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var isSuccess = await _employeeService.DeleteEmployee(new Employee { Id = id });
            if (!isSuccess)
                NotFound();
            return Ok();
        }
    }
}