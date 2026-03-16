using FluentValidation;

namespace NYC360.Application.Features.Posts.Queries.PostDetails;

public sealed class PostGetDetailsQueryValidator : AbstractValidator<PostGetDetailsQuery>
{
    public PostGetDetailsQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Post ID must be greater than zero.");
    }
}