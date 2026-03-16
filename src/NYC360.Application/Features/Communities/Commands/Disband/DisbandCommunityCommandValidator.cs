using FluentValidation;

namespace NYC360.Application.Features.Communities.Commands.Disband;

public class DisbandCommunityCommandValidator : AbstractValidator<DisbandCommunityCommand>
{
    public DisbandCommunityCommandValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("A valid user ID is required.");

        RuleFor(x => x.CommunityId)
            .GreaterThan(0)
            .WithMessage("A valid community ID is required.");
    }
}
