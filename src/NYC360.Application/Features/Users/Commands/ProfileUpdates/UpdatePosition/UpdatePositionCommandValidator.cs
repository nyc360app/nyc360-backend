using FluentValidation;

namespace NYC360.Application.Features.Users.Commands.ProfileUpdates.UpdatePosition;

public class UpdatePositionCommandValidator : AbstractValidator<UpdatePositionCommand>
{
    public UpdatePositionCommandValidator()
    {
        RuleFor(x => x.PositionId).NotEmpty();
        RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Company).NotEmpty().MaximumLength(100);
        RuleFor(x => x.StartDate).LessThanOrEqualTo(DateTime.UtcNow);
        
        RuleFor(x => x)
            .Must(x => x.EndDate == null || x.EndDate > x.StartDate)
            .WithMessage("End date must be after the start date.");
    }
}