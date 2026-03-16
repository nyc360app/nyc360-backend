using FluentValidation;

namespace NYC360.Application.Features.Communities.Commands.UpdateMemberRole;

public class UpdateMemberRoleCommandValidator : AbstractValidator<UpdateMemberRoleCommand>
{
    public UpdateMemberRoleCommandValidator()
    {
        RuleFor(x => x.UserId).GreaterThan(0);
        RuleFor(x => x.CommunityId).GreaterThan(0);
        RuleFor(x => x.TargetUserId).GreaterThan(0);
        RuleFor(x => x.NewRole).IsInEnum();
    }
}
