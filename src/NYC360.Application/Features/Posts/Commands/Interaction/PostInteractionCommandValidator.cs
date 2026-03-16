using FluentValidation;

namespace NYC360.Application.Features.Posts.Commands.Interaction;

public sealed class PostInteractionCommandValidator : AbstractValidator<PostInteractionCommand>
{
    public PostInteractionCommandValidator()
    {
        RuleFor(x => x.UserId).GreaterThan(0);
        RuleFor(x => x.PostId).GreaterThan(0);
        RuleFor(x => x.Interaction).IsInEnum();
    }
}