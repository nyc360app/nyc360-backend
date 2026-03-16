using FluentValidation;

namespace NYC360.Application.Features.Authentication.Commands.TwoFactorVerify;

public class TwoFactorVerifyCommandValidator : AbstractValidator<TwoFactorVerifyCommand>
{
    public TwoFactorVerifyCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Code).NotEmpty().Length(6);
    }
}