using NYC360.Application.Contracts.Persistence;
using FluentValidation;

namespace NYC360.Application.Features.Posts.Commands.SavePost;
    
public class SavePostCommandValidator : AbstractValidator<SavePostCommand>
{
    private readonly IPostRepository _postRepository;

    public SavePostCommandValidator(IPostRepository postRepository)
    {
        _postRepository = postRepository;

        RuleFor(p => p.PostId)
            .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.");
        
        // Assuming UserId comes from authentication and is always valid for this context
        RuleFor(p => p.UserId)
            .GreaterThan(0).WithMessage("User ID must be greater than 0.");
        
        // Custom validation for Post existence, only if PostId is valid
        RuleFor(p => p.PostId)
            .MustAsync(PostMustExist).WithMessage("Post not found.")
            .When(p => p.PostId > 0);
    }

    private async Task<bool> PostMustExist(int postId, CancellationToken cancellationToken)
    {
        return await _postRepository.ExistsAsync(postId, cancellationToken);
    }
}
