using Domain.Models;
using FluentValidation;

namespace Domain.Services.Validation
{
    public class PositionValidator : AbstractValidator<Position>
    {
        private readonly IUnitOfWork _dbContext;

        public PositionValidator(IUnitOfWork dbContext)
        {
            _dbContext = dbContext;
            RuleFor(p => p.PositionName).NotEmpty();
        }
    }
}
