using FluentValidation;

namespace NYC360.Application.Features.Users.Commands.ProfileUpdates.AddPosition;

public class AddPositionCommandValidator : AbstractValidator<AddPositionCommand>
{
    public AddPositionCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().MaximumLength(100);

        RuleFor(x => x.Company)
            .NotEmpty().MaximumLength(100);

        RuleFor(x => x.StartDate)
            .NotEmpty()
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Start date cannot be in the future.");

        RuleFor(x => x)
            .Must(x => x.EndDate == null || x.EndDate > x.StartDate)
            .WithMessage("End date must be after the start date.");

        RuleFor(x => x)
            .Must(x => !x.IsCurrent || x.EndDate == null)
            .WithMessage("Current positions should not have an end date.");
    }
}