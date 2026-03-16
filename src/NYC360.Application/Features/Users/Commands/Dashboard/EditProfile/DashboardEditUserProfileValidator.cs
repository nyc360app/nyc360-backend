using FluentValidation;

namespace NYC360.Application.Features.Users.Commands.Dashboard.EditProfile;

public class DashboardEditUserProfileValidator : AbstractValidator<DashboardEditUserProfileCommand>
{
    public DashboardEditUserProfileValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.FirstName).MaximumLength(100);
        RuleFor(x => x.LastName).MaximumLength(100);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Bio).MaximumLength(500);
    }
}