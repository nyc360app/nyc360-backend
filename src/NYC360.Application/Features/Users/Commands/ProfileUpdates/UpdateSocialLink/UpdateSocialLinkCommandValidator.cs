using FluentValidation;

namespace NYC360.Application.Features.Users.Commands.ProfileUpdates.UpdateSocialLink;

public class UpdateSocialLinkCommandValidator: AbstractValidator<UpdateSocialLinkCommand>
{
    public UpdateSocialLinkCommandValidator()
    {
        // Ensure the ID of the link to update is provided
        RuleFor(x => x.LinkId)
            .GreaterThan(0)
            .WithMessage("A valid Link ID is required.");

        // Validate the platform
        RuleFor(x => x.Platform)
            .IsInEnum()
            .WithMessage("Please select a valid social platform.");

        // Validate the URL
        RuleFor(x => x.Url)
            .NotEmpty().WithMessage("URL is required.")
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
            .WithMessage("A valid absolute URL (including http/https) is required.");
    }
}