using FluentValidation;

namespace NYC360.Application.Features.Users.Queries.List;

public class GetUsersListQueryValidator : AbstractValidator<GetUsersListQuery>
{
    public GetUsersListQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(5, 100);
    }
}