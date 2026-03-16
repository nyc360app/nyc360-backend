using FluentValidation;

namespace NYC360.Application.Features.Posts.Commands.SubmitFlag;

public class SubmitPostFlagCommandValidator : AbstractValidator<SubmitPostFlagCommand>
{
    public SubmitPostFlagCommandValidator()
    {
        RuleFor(x => x.PostId)
            .GreaterThan(0).WithMessage("Post ID is required.");

        RuleFor(x => x.UserId)
            .GreaterThan(0).WithMessage("User ID is required.");
        
        // Ensure the reason is a valid enum value
        RuleFor(x => x.Reason)
            .IsInEnum().WithMessage("A valid flag reason is required.");
        
        RuleFor(x => x.Details)
            .MaximumLength(500).WithMessage("Details cannot exceed 500 characters.");
    }
}