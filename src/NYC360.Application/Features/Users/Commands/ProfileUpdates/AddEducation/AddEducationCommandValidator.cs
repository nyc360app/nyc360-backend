using FluentValidation;

namespace NYC360.Application.Features.Users.Commands.ProfileUpdates.AddEducation;

public class AddEducationCommandValidator: AbstractValidator<AddEducationCommand>
{
    public AddEducationCommandValidator()
    {
        RuleFor(x => x.School).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Degree).NotEmpty().MaximumLength(100);
        RuleFor(x => x.FieldOfStudy).NotEmpty().MaximumLength(100);
        
        RuleFor(x => x.StartDate)
            .NotEmpty()
            .LessThanOrEqualTo(DateTime.UtcNow);

        RuleFor(x => x)
            .Must(x => x.EndDate == null || x.EndDate > x.StartDate)
            .WithMessage("End date must be after the start date.");
    }
}