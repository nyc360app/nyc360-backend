using FluentValidation;

namespace NYC360.Application.Features.Professions.Commands.Apply;

public class ApplyToJobCommandValidator : AbstractValidator<ApplyToJobCommand>
{
    public ApplyToJobCommandValidator()
    {
        RuleFor(x => x.JobOfferId).GreaterThan(0);
        RuleFor(x => x.CoverLetter).MaximumLength(3000);
    }
}