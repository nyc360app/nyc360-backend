using FluentValidation;
using NYC360.Domain.Enums.SpaceListings;

namespace NYC360.Application.Features.SpaceListings.Commands.Submit;

public class SubmitSpaceListingCommandValidator : AbstractValidator<SubmitSpaceListingCommand>
{
    public SubmitSpaceListingCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(2000);

        RuleFor(x => x.Address.ZipCode)
            .NotEmpty()
            .MaximumLength(10);

        RuleFor(x => x.ContactName)
            .MaximumLength(100)
            .When(x => !string.IsNullOrWhiteSpace(x.ContactName));

        RuleFor(x => x.SubmitterNote)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrWhiteSpace(x.SubmitterNote));

        RuleFor(x => x)
            .Must(HasContactMethod)
            .WithMessage("At least one contact method is required for place, business, or organization listings.")
            .When(x => x.EntityType is SpaceListingEntityType.Place or SpaceListingEntityType.Business or SpaceListingEntityType.Organization);
    }

    private static bool HasContactMethod(SubmitSpaceListingCommand request)
        => !string.IsNullOrWhiteSpace(request.Website)
           || !string.IsNullOrWhiteSpace(request.PhoneNumber)
           || !string.IsNullOrWhiteSpace(request.PublicEmail);
}
