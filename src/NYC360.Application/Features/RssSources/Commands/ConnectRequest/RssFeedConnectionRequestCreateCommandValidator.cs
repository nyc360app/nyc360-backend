using FluentValidation;

namespace NYC360.Application.Features.RssSources.Commands.ConnectRequest;

public class RssFeedConnectionRequestCreateCommandValidator : AbstractValidator<RssFeedConnectionRequestCreateCommand>
{
    public RssFeedConnectionRequestCreateCommandValidator()
    {
        RuleFor(x => x.Url)
            .NotEmpty().WithMessage("URL is required.")
            .MaximumLength(500).WithMessage("URL is too long.")
            .Must(BeAValidUrl).WithMessage("Invalid URL format.");

        RuleFor(x => x.Category)
            .IsInEnum().WithMessage("Invalid category.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name is too long.");
    }

    private bool BeAValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out var result)
               && (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps);
    }
}
