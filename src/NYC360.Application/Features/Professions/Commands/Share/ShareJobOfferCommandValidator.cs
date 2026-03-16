using FluentValidation;

namespace NYC360.Application.Features.Professions.Commands.Share;

public class ShareJobOfferCommandValidator : AbstractValidator<ShareJobOfferCommand>
{
    public ShareJobOfferCommandValidator()
    {
        // Must point to a real JobOffer resource
        RuleFor(x => x.JobOfferId)
            .GreaterThan(0).WithMessage("A valid Job Offer ID is required to share.");

        // Social Content requirements
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Please add some text to your post.")
            .MinimumLength(10).WithMessage("Share description is too short.")
            .MaximumLength(1000).WithMessage("Share description cannot exceed 1000 characters.");

        // Tags validation (Optional but restricted)
        RuleFor(x => x.Tags)
            .Must(t => t == null || t.Count <= 5).WithMessage("You can only add up to 5 tags.");

        // Community logic (If provided)
        RuleFor(x => x.CommunityId)
            .GreaterThan(0).When(x => x.CommunityId.HasValue)
            .WithMessage("Invalid Community selection.");
    }
}