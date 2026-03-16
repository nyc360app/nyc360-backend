using FluentValidation;

namespace NYC360.Application.Features.Users.Commands.Dashboard.Delete;

public class DeleteUserValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserValidator()
    {
        RuleFor(x => x.UserId).GreaterThan(0);
    }
}