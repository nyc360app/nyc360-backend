using FluentValidation;

namespace NYC360.Application.Features.Users.Commands.ProfileUpdates.UpdateEducation;

public class UpdateEducationCommandValidator: AbstractValidator<UpdateEducationCommand>
{
    public UpdateEducationCommandValidator()
    {
        RuleFor(x => x.EducationId).NotEmpty();
        RuleFor(x => x.School).NotEmpty().MaximumLength(150);
        RuleFor(x => x.StartDate).LessThanOrEqualTo(DateTime.UtcNow);
        RuleFor(x => x).Must(x => x.EndDate == null || x.EndDate > x.StartDate)
            .WithMessage("End date must be after the start date.");
    }
}