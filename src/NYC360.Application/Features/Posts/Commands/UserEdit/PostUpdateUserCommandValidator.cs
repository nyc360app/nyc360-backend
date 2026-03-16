using FluentValidation;

namespace NYC360.Application.Features.Posts.Commands.UserEdit;

public class PostUpdateUserCommandValidator : AbstractValidator<PostUpdateUserCommand>
{
    public PostUpdateUserCommandValidator()
    {
        RuleFor(x => x.PostId).GreaterThan(0);
        RuleFor(x => x.UserId).GreaterThan(0);

        RuleFor(x => x.Title)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(200);

        RuleFor(x => x.Content)
            .NotEmpty()
            .MinimumLength(10);

        RuleFor(x => x.Category)
            .IsInEnum();

        RuleFor(x => x.TopicId)
            .GreaterThan(0)
            .When(x => x.TopicId.HasValue);
    }
}