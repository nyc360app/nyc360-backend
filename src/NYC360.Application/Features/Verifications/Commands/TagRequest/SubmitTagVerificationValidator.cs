using FluentValidation;

namespace NYC360.Application.Features.Verifications.Commands.TagRequest;

public class SubmitTagVerificationValidator: AbstractValidator<SubmitTagVerificationCommand>
{
    public SubmitTagVerificationValidator()
    {
        RuleFor(x => x.TagId).GreaterThan(0);
        
        RuleFor(x => x.Reason)
            .NotEmpty()
            .MinimumLength(20)
            .MaximumLength(5000)
            .WithMessage("Please provide a valid reason (20-5000 chars).");

        RuleFor(x => x.File)
            .NotNull()
            .Must(f => f.Length > 0 && f.Length < 5 * 1024 * 1024)
            .WithMessage("A file under 5MB is required.");
            
        RuleFor(x => x.DocumentType).IsInEnum();
    }
}
