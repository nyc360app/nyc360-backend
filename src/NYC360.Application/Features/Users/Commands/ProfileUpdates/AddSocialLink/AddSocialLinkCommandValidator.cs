using FluentValidation;

namespace NYC360.Application.Features.Users.Commands.ProfileUpdates.AddSocialLink;

public class AddSocialLinkCommandValidator : AbstractValidator<AddSocialLinkCommand>
{
    public AddSocialLinkCommandValidator()
    {
        RuleFor(x => x.Url)
            .NotEmpty()
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
            .WithMessage("A valid absolute URL is required.");
        
        RuleFor(x => x.Platform).IsInEnum();
    }
}