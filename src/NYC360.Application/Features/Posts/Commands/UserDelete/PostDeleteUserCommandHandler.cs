using NYC360.Application.Contracts.Persistence;
using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Posts.Commands.UserDelete;

public class PostDeleteUserCommandHandler(
    IPostRepository postRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<PostDeleteUserCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(PostDeleteUserCommand request, CancellationToken ct)
    {
        var post = await postRepository.GetByIdAsync(request.PostId, ct);
        if (post is null)
            return StandardResponse.Failure(new("post.notfound", "Post not found."));
        
        if (post.AuthorId != request.UserId)
            return StandardResponse.Failure(new("post.forbidden", "You cannot delete this post."));

        postRepository.Remove(post);
        await unitOfWork.SaveChangesAsync(ct);

        return StandardResponse.Success();
    }
}