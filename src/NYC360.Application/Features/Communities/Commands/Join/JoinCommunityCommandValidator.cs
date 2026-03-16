using FluentValidation;

namespace NYC360.Application.Features.Communities.Commands.Join;

public class JoinCommunityCommandValidator : AbstractValidator<JoinCommunityCommand>
{
    public JoinCommunityCommandValidator()
    {
        RuleFor(x => x.CommunityId)
            .GreaterThan(0);
    }
}