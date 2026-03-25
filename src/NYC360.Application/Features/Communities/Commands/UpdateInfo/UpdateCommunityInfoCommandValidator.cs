using Microsoft.AspNetCore.Http;
using FluentValidation;

namespace NYC360.Application.Features.Communities.Commands.UpdateInfo;

public class UpdateCommunityInfoCommandValidator : AbstractValidator<UpdateCommunityInfoCommand>
{
    public UpdateCommunityInfoCommandValidator()
    {
        RuleFor(x => x.CommunityId)
            .GreaterThan(0)
            .WithMessage("A valid community ID is required.");

        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("A valid user ID is required.");

        RuleFor(x => x.Name)
            .MinimumLength(3)
            .MaximumLength(100)
            .When(x => !string.IsNullOrWhiteSpace(x.Name))
            .WithMessage("Name must be between 3 and 100 characters.");

        RuleFor(x => x.Description)
            .MinimumLength(10)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrWhiteSpace(x.Description))
            .WithMessage("Description must be between 10 and 1000 characters.");

        RuleFor(x => x.Rules)
            .MaximumLength(4000)
            .When(x => x.Rules != null)
            .WithMessage("Rules must be 4000 characters or fewer.");

        RuleFor(x => x.AvatarImage)
            .Must(BeValidImage!)
            .When(x => x.AvatarImage != null)
            .WithMessage("Avatar must be a valid image (JPEG, PNG, or WebP) under 5MB.");

        RuleFor(x => x.CoverImage)
            .Must(BeValidImage!)
            .When(x => x.CoverImage != null)
            .WithMessage("Cover image must be a valid image (JPEG, PNG, or WebP) under 5MB.");
    }

    private static bool BeValidImage(IFormFile file)
    {
        var allowed = new[] { "image/jpeg", "image/png", "image/webp" };
        return allowed.Contains(file.ContentType) && file.Length <= 5_000_000;
    }
}
