using NYC360.Application.Contracts.Persistence;
using FluentValidation;

namespace NYC360.Application.Features.Posts.Commands.Share;

public class PostShareCommandValidator : AbstractValidator<PostShareCommand>
{
    public PostShareCommandValidator(ILocationRepository locationRepository)
    {
        RuleFor(x => x.ParentPostId)
            .GreaterThan(0)
            .WithMessage("A valid post must be selected to share.");

        RuleFor(x => x.Commentary)
            .MaximumLength(1000)
            .WithMessage("Commentary cannot exceed 1000 characters.");
    }
}