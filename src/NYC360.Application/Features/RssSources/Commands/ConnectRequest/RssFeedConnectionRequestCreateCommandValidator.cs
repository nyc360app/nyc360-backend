using FluentValidation;
using Microsoft.AspNetCore.Http;

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

        RuleFor(x => x.Language)
            .MaximumLength(50).WithMessage("Language is too long.")
            .When(x => !string.IsNullOrWhiteSpace(x.Language));

        RuleFor(x => x.SourceWebsite)
            .MaximumLength(500).WithMessage("Source website URL is too long.")
            .Must(BeAValidUrl).WithMessage("Invalid source website URL format.")
            .When(x => !string.IsNullOrWhiteSpace(x.SourceWebsite));

        RuleFor(x => x.SourceCredibility)
            .MaximumLength(100).WithMessage("Source credibility is too long.")
            .When(x => !string.IsNullOrWhiteSpace(x.SourceCredibility));

        RuleFor(x => x.DivisionTag)
            .MaximumLength(50).WithMessage("Division tag is too long.")
            .When(x => !string.IsNullOrWhiteSpace(x.DivisionTag));

        RuleFor(x => x.AgreementAccepted)
            .Equal(true)
            .WithMessage("Agreement must be accepted.");

        RuleFor(x => x.LogoImage)
            .Must(BeValidImage!)
            .When(x => x.LogoImage != null)
            .WithMessage("Logo image must be JPEG, PNG, or WebP and <= 5MB.");
    }

    private bool BeAValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out var result)
               && (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps);
    }

    private static bool BeValidImage(IFormFile file)
    {
        var allowed = new[] { "image/jpeg", "image/png", "image/webp" };
        return allowed.Contains(file.ContentType) && file.Length <= 5_000_000;
    }
}
