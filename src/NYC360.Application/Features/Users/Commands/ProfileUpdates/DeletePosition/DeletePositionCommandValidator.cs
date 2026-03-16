using FluentValidation;

namespace NYC360.Application.Features.Users.Commands.ProfileUpdates.DeletePosition;

public class DeletePositionCommandValidator: AbstractValidator<DeletePositionCommand>
{
    public DeletePositionCommandValidator()
    {
        RuleFor(x => x.PositionId).GreaterThan(0);
        RuleFor(x => x.UserId).GreaterThan(0);
    }
}