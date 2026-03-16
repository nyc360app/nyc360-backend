using FluentValidation;

namespace NYC360.Application.Features.Authentication.Commands.PasswordReset;

public class PasswordResetCommandValidator : AbstractValidator<PasswordResetCommand>
{
    public PasswordResetCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.NewPassword).NotEmpty().MinimumLength(6);
        RuleFor(x => x.Token).NotEmpty();
    }
}