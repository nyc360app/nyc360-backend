using FluentValidation;

namespace NYC360.Application.Features.Users.Commands.ProfileUpdates.UploadAvatar;

public class UploadAvatarCommandValidator: AbstractValidator<UploadAvatarCommand>
{
    public UploadAvatarCommandValidator()
    {
        RuleFor(x => x.File).NotNull();
        
        RuleFor(x => x.File.Length)
            .LessThanOrEqualTo(5 * 1024 * 1024) // 5MB Limit
            .WithMessage("Image size must be less than 5MB.");

        RuleFor(x => x.File.ContentType)
            .Must(x => x.Equals("image/jpeg") || x.Equals("image/png") || x.Equals("image/webp"))
            .WithMessage("Only JPG, PNG and WebP images are allowed.");
    }
}