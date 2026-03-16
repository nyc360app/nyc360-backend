using FluentValidation;

namespace NYC360.Application.Features.Users.Commands.ProfileUpdates.DeleteSocialLink;

public class DeleteSocialLinkCommandValidator : AbstractValidator<DeleteSocialLinkCommand>
{
    public DeleteSocialLinkCommandValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("User ID must be valid.");

        RuleFor(x => x.LinkId)
            .GreaterThan(0)
            .WithMessage("Social Link ID must be valid.");
    }
}