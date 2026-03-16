using FluentValidation;

namespace NYC360.Application.Features.Posts.Commands.DashboardEdit;

public class PostUpdateDashboardCommandValidator : AbstractValidator<PostUpdateDashboardCommand>
{
    public PostUpdateDashboardCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);

        RuleFor(x => x.Title)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(200);

        RuleFor(x => x.Content)
            .NotEmpty()
            .MinimumLength(10);

        RuleFor(x => x.Category)
            .IsInEnum()
            .WithMessage("Invalid category");

        RuleFor(x => x.TopicId)
            .GreaterThan(0)
            .When(x => x.TopicId.HasValue);

        // RuleFor(x => x.Image)
        //     .SetValidator(new ImageValidator()!)
        //     .When(x => x.Image is not null);
    }
}