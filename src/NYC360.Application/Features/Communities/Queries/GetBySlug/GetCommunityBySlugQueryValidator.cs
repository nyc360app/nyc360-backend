using FluentValidation;

namespace NYC360.Application.Features.Communities.Queries.GetBySlug;

public class GetCommunityBySlugQueryValidator : AbstractValidator<GetCommunityBySlugQuery>
{
    public GetCommunityBySlugQueryValidator()
    {
        RuleFor(x => x.Slug)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 50);
    }
}
