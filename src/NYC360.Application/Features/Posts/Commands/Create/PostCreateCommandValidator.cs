using FluentValidation;

namespace NYC360.Application.Features.Posts.Commands.Create;

public class PostCreateCommandValidator : AbstractValidator<PostCreateCommand>
{
    public PostCreateCommandValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0);

        RuleFor(x => x.Title)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(200);

        RuleFor(x => x.Content)
            .NotEmpty()
            .MinimumLength(10);

        // Topic is optional
        RuleFor(x => x.TopicId)
            .GreaterThan(0)
            .When(x => x.TopicId.HasValue);

        // RuleFor(x => x.Image)
        //     .SetValidator(new ImageValidator())
        //     .When(x => x.Image is not null);
    }
}