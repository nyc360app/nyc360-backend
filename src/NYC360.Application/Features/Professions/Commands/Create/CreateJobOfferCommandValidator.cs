using FluentValidation;

namespace NYC360.Application.Features.Professions.Commands.Create;

public class CreateJobOfferCommandValidator : AbstractValidator<CreateJobOfferCommand>
{
    public CreateJobOfferCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Job title is required.")
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Job description is required.");

        // Salary Logic
        RuleFor(x => x.SalaryMin)
            .GreaterThanOrEqualTo(0).When(x => x.SalaryMin.HasValue)
            .WithMessage("Minimum salary cannot be negative.");

        RuleFor(x => x.SalaryMax)
            .GreaterThanOrEqualTo(x => x.SalaryMin ?? 0)
            .When(x => x.SalaryMax.HasValue && x.SalaryMin.HasValue)
            .WithMessage("Maximum salary must be greater than or equal to minimum salary.");

        // Enum Validations
        RuleFor(x => x.WorkArrangement)
            .IsInEnum().WithMessage("Invalid work arrangement selection.");

        RuleFor(x => x.EmploymentType)
            .IsInEnum().WithMessage("Invalid employment type selection.");
            
        // Requirements & Benefits (Optional but restricted)
        RuleFor(x => x.Requirements)
            .MaximumLength(2000).WithMessage("Requirements text is too long.");

        RuleFor(x => x.Benefits)
            .MaximumLength(2000).WithMessage("Benefits text is too long.");
    }
}