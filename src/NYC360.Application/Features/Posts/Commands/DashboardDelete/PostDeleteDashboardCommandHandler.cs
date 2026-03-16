using NYC360.Application.Contracts.Persistence;
using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Posts.Commands.DashboardDelete;

public class PostDeleteDashboardCommandHandler(
    IPostRepository postRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<PostDeleteAdminCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(PostDeleteAdminCommand request, CancellationToken ct)
    {
        var post = await postRepository.GetByIdAsync(request.PostId, ct);
        if (post is null)
            return StandardResponse.Failure(new ApiError("post.notfound", "Post not found."));

        postRepository.Remove(post);
        await unitOfWork.SaveChangesAsync(ct);

        return StandardResponse.Success();
    }
}