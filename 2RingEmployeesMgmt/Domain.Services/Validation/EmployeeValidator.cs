using Domain.Models;
using FluentValidation;
using System;

namespace Domain.Services.Validation
{
    public class EmployeeValidator : AbstractValidator<Employee>
    {
        public EmployeeValidator()
        {
            RuleFor(e => e.FirstName).NotEmpty();
            RuleFor(e => e.Surname).NotEmpty();
            RuleFor(e => e.Birthday).NotEmpty();
            RuleFor(e => e.Birthday).Must((e, birthday) => IsDateFromPast(birthday)).WithMessage($"Employee {nameof(Employee.Birthday)} is not date from past");
            RuleFor(e => e.StartDate).NotEmpty();
            RuleFor(e => e.StartDate).Must((e, startdate) => IsDateFromFuture(startdate)).WithMessage($"Employee {nameof(Employee.StartDate)} already passed");
            RuleFor(e => e.Position).NotEmpty();
            RuleFor(e => e.Position).ChildRules(p =>
            {
                p.RuleFor(pos => pos.Id).NotEmpty();
            });
            RuleFor(e => e.Salary).NotEmpty();
        }

        private static bool IsDateFromPast(DateTime dayTime)
        {
            return dayTime < DateTime.UtcNow;
        }

        private static bool IsDateFromFuture(DateTime dayTime)
        {
            var newDate = DateTime.UtcNow;
            var isValid = dayTime.Year >= newDate.Year && dayTime.Month >= newDate.Month && dayTime.Day >= newDate.Day;

            return isValid;
        }
    }
}
