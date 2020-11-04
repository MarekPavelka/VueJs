using Domain.Models;
using Domain.Services;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class EmployeeServiceTests : InMemoryDbTest
    {
        private readonly EmployeeService _testObject;
        private readonly Position _defaultPosition = new Position { Id = 1, PositionName = "Tester", };

        public EmployeeServiceTests()
        {

            _testObject = new EmployeeService(DbContext);
        }

        [Fact]
        public async Task GivenEmployeeDto_WhenAddNewEmployeeCalled_ThenEmployeeSavedToDb()
        {
            DbContext.Positions.Add(_defaultPosition);
            DbContext.SaveChanges();
            DetachAllEntites();
            var newEmployee = new Employee { FirstName = "Bob", Position = new Position { Id = 1 } };

            await _testObject.AddNewEmployee(newEmployee);

            DbContext.Employees.Should().ContainSingle().Which.Should().Be(newEmployee);
        }

        [Fact]
        public async Task GivenEmployeeDto_WhenUpdateEmployeeCalled_ThenEmployeeUpdatedInDB()
        {
            DbContext.Positions.Add(_defaultPosition);
            DetachAllEntites();
            var newEmployee = new Employee { Id = 1, FirstName = "Bob", Position = new Position { Id = 1 } };
            DbContext.Employees.Add(newEmployee);
            DbContext.SaveChanges();
            DetachAllEntites();
            newEmployee.FirstName = "Max";

            await _testObject.UpdateEmployee(newEmployee);

            DbContext.Employees.Should().ContainSingle().Which.FirstName.Should().Be("Max");
        }

        [Fact]
        public async Task WhenArchiveEmployeeCalled_ThenEmployeeArchivedPropertyChangesInDB()
        {
            DbContext.Positions.Add(_defaultPosition);
            var validEmployees = GetValidEmployees(_defaultPosition);
            validEmployees.ForEach(e =>
            {
                DbContext.Add(e);
            });
            DbContext.SaveChanges();

            await _testObject.ArchiveEmployee(2);

            DbContext.Employees.Should().ContainSingle(e => e.IsArchived);
        }

        [Fact]
        public async Task WhenDeleteEmployeeCalled_ThenEmployeeIsDeletedFromDB()
        {
            DbContext.Positions.Add(new Position { Id = 1, PositionName = "Tester", });
            DetachAllEntites();
            var newEmployee = new Employee { FirstName = "Bob", Position = new Position { Id = 1 }, IsArchived = false };
            DbContext.Employees.Add(newEmployee);
            DbContext.SaveChanges();
            DetachAllEntites();

            await _testObject.DeleteEmployee(newEmployee);

            DbContext.Employees.Should().BeEmpty();
        }
        [Fact]
        public async Task WhenGetArchivedEmployeesCalled_ThenRetrieveArchivedEmployeesFromDB()
        {
            var position = new Position { Id = 1, PositionName = "Tester", };
            DbContext.Positions.Add(position);
            DbContext.Add(new Employee { Id = 1, FirstName = "Bob", Position = position, IsArchived = true });
            DbContext.Add(new Employee { Id = 2, FirstName = "Max", Position = position, IsArchived = false });
            DbContext.Add(new Employee { Id = 3, FirstName = "Pedro", Position = position, IsArchived = false });
            DbContext.SaveChanges();

            var archivedEmployees = await _testObject.GetArchivedEmployees();

            archivedEmployees.Should().OnlyContain(e => e.IsArchived == true);
        }

        [Fact]
        public async Task WhenGetCurrentEmployeesCalled_ThenRetrieveEmployeesWithIsArchivedFalseFromDB()
        {
            DbContext.Positions.Add(_defaultPosition);
            var employees = GetValidEmployees(_defaultPosition);
            employees.ForEach(e => DbContext.Add(e));
            DbContext.SaveChanges();

            var allEmployees = await _testObject.GetCurrentEmployees();

            allEmployees.Count().Should().Be(3);
        }

        [Fact]
        public async Task WhenGetEmployeeCalledWithIdProvided_ThenRetrieveEmployeeWithSameIdFromDB()
        {
            DbContext.Positions.Add(_defaultPosition);
            var employees = GetValidEmployees(_defaultPosition);
            employees.ForEach(e => DbContext.Add(e));
            DbContext.SaveChanges();

            var retrievedEmployee = await _testObject.GetEmployee(2);

            retrievedEmployee.FirstName.Should().Be("Max");
        }

        [Fact]
        public async Task GivenEmployeeDto_WhenAddEmployeeCalled_ThenEmployeePositionHistoryCreatedInDB()
        {
            DbContext.Positions.Add(_defaultPosition);
            DbContext.SaveChanges();
            DetachAllEntites();
            var newEmployee = new Employee { FirstName = "Bob", Position = new Position { Id = 1 } };

            await _testObject.AddNewEmployee(newEmployee);

            DbContext.PositionHistory.Should().ContainSingle();
        }

        [Fact]
        public async Task WhenUpdateEmployeeCalled_ThenEmployeePositionHistoryUpdatesInDB()
        {
            var newPosition = new Position { Id = 2, PositionName = "Salesman" };
            var newEmployee = new Employee { Id = 1, FirstName = "Bob", Position = _defaultPosition };
            DbContext.Positions.Add(_defaultPosition);
            DbContext.Positions.Add(newPosition);
            DbContext.SaveChanges();
            DetachAllEntites();
            await _testObject.AddNewEmployee(newEmployee);

            await _testObject.UpdateEmployee(new Employee { Id = 1, FirstName = "Bob", Position = newPosition});

            DbContext.PositionHistory.Count().Should().Be(2);
            DbContext.PositionHistory.Should().ContainSingle(ph => ph.EndDate == null).Which.Position.PositionName.Should().Be("Salesman");
        }

        private List<Employee> GetValidEmployees(Position position)
        {
            var employee1 = new Employee { Id = 1, FirstName = "Bob", Position = position, IsArchived = false };
            var employee2 = new Employee { Id = 2, FirstName = "Max", Position = position, IsArchived = false };
            var employee3 = new Employee { Id = 3, FirstName = "Pedro", Position = position, IsArchived = false };

            return new List<Employee> { employee1, employee2, employee3 };
        }
    }
}