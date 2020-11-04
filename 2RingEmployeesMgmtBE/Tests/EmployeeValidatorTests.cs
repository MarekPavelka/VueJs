using Domain.Models;
using Domain.Services.Validation;
using FluentAssertions;
using System;
using Xunit;

namespace Tests
{
    public class EmployeeValidatorTests
    {
        private readonly EmployeeValidator _testObject = new EmployeeValidator();

        [Fact]
        public void GivenValidEmployee_WhenValidateCalled_ThenValidResultReturned()
        {
            var validEmployee = GetValidEmployee();

            var result = _testObject.Validate(validEmployee);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void WhenEmployeeWithBirthdayInFurtureValidated_ThenValidationErrorReturned()
        {
            var testEmployee = GetValidEmployee();
            testEmployee.Birthday = DateTime.Now.AddMonths(1);

            var result = _testObject.Validate(testEmployee);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().Which.ErrorMessage.Should().ContainAll("Birthday", "past");
        }

        [Fact]
        public void WhenEmployeeStartDateInPastValidated_ThenValidationErrorReturned()
        {
            var testEmployee = GetValidEmployee();
            var newDate = DateTime.UtcNow;
            testEmployee.StartDate = newDate.Subtract(new TimeSpan(1, 0, 0, 0));

            var result = _testObject.Validate(testEmployee);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().Which.ErrorMessage.Should().ContainAll("StartDate", "passed");
        }

        private Employee GetValidEmployee() =>
            new Employee
            {
                FirstName = "Bob",
                Surname = "Chewie",
                Birthday = new DateTime(2010, 1, 1),
                StartDate = DateTime.Now.AddMonths(1),
                Position = new Position { Id = 1 },
                Salary = 2000
            };
    }
}
