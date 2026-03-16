using FluentValidation;

namespace NYC360.Application.Features.Communities.Commands.Leave;

public class LeaveCommunityCommandValidator : AbstractValidator<LeaveCommunityCommand>
{
    public LeaveCommunityCommandValidator()
    {
        RuleFor(x => x.CommunityId)
            .GreaterThan(0);
    }
}