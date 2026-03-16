using FluentValidation;

namespace NYC360.Application.Features.Posts.Queries.List;

public class PostsGetPagedQueryValidator : AbstractValidator<PostsGetPagedQuery>
{
    public PostsGetPagedQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.PageSize).GreaterThan(0).LessThanOrEqualTo(500);
    }
}