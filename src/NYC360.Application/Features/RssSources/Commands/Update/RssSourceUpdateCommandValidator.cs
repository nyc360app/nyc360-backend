using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace NYC360.Application.Features.RssSources.Commands.Update;

public class RssSourceUpdateCommandValidator : AbstractValidator<RssSourceUpdateCommand>
{
    public RssSourceUpdateCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);

        RuleFor(x => x.RssUrl)
            .NotEmpty()
            .MaximumLength(500)
            .Must(BeAValidUrl)
            .WithMessage("Invalid RSS URL format.");

        RuleFor(x => x.Category)
            .IsInEnum();

        RuleFor(x => x.Name)
            .MaximumLength(150)
            .When(x => x.Name is not null);

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .When(x => x.Description is not null);

        // RuleFor(x => x.Image)
        //     .Must(BeValidImage)
        //     .When(x => x.Image is not null)
        //     .WithMessage("Image must be PNG or JPG and less than 2MB.");
    }

    private bool BeAValidUrl(string url)
        => Uri.TryCreate(url, UriKind.Absolute, out var r) &&
           (r.Scheme == Uri.UriSchemeHttp || r.Scheme == Uri.UriSchemeHttps);

    private bool BeValidImage(IFormFile file)
        => file.Length <= 2 * 1024 * 1024 &&
           (file.ContentType == "image/png" || file.ContentType == "image/jpeg");
}