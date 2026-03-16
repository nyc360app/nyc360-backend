using FluentValidation;

namespace NYC360.Application.Features.Professions.Commands.UpdateOffer;

public class UpdateJobOfferCommandValidator : AbstractValidator<UpdateJobOfferCommand>
{
    public UpdateJobOfferCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.SalaryMax).GreaterThanOrEqualTo(x => x.SalaryMin ?? 0)
            .When(x => x.SalaryMax.HasValue && x.SalaryMin.HasValue);
        RuleFor(x => x.WorkArrangement).IsInEnum();
        RuleFor(x => x.EmploymentType).IsInEnum();
    }
}