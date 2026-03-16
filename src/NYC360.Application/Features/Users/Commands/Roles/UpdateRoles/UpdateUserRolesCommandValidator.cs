using FluentValidation;

namespace NYC360.Application.Features.Users.Commands.Roles.UpdateRoles;

public class UpdateUserRolesCommandValidator : AbstractValidator<UpdateUserRolesCommand>
{
    public UpdateUserRolesCommandValidator()
    {
        RuleFor(x => x.UserId).GreaterThan(0);
        RuleFor(x => x.RoleName).NotNull();
    }
}