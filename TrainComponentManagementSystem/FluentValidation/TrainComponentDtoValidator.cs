using FluentValidation;
using TrainComponentManagementSystem.Models;

namespace TrainComponentManagementSystem.FluentValidation
{
    public class TrainComponentDtoValidator : AbstractValidator<TrainComponentDto>
    {
        public TrainComponentDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.UniqueNumber).NotEmpty();
            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .When(x => x.CanAssignQuantity && x.Quantity.HasValue)
                .WithMessage("Quantity must be a positive integer if CanAssignQuantity is true.");
        }
    }
}
