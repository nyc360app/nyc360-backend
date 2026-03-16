using FluentValidation;

namespace NYC360.Application.Features.Users.Commands.Roles.UpdatePermissions;

public class UpdateUserPermissionsCommandValidator : AbstractValidator<UpdateUserPermissionsCommand>
{
    public UpdateUserPermissionsCommandValidator()
    {
        RuleFor(x => x.UserId).GreaterThan(0);
        RuleFor(x => x.Permissions).NotNull();
    }
}