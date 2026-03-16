using NYC360.Domain.Entities.Posts;

namespace NYC360.Application.Contracts.Persistence;

public interface IPostCommentRepository
{
    Task<PostComment?> GetCommentByIdAsync(int commentId, CancellationToken ct);
    Task AddCommentAsync(PostComment comment, CancellationToken ct);
}