using FluentValidation;

namespace NYC360.Application.Features.Communities.Commands.CreatePost;

public class CreateCommunityPostCommandValidator : AbstractValidator<CreateCommunityPostCommand>
{
    public CreateCommunityPostCommandValidator()
    {
        RuleFor(x => x.CommunityId)
            .GreaterThan(0)
            .WithMessage("Community ID is required.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content is required.");
            
        // Optional: File size validation
        RuleFor(x => x.Attachments)
            .Must(files => files == null || files.Count <= 5)
            .WithMessage("You can only upload up to 5 attachments.");
    }
}