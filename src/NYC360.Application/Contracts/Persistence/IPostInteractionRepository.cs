using NYC360.Domain.Entities.Posts;

namespace NYC360.Application.Contracts.Persistence;

public interface IPostInteractionRepository
{
    Task<PostInteraction?> GetInteractionAsync(int postId, int userId, CancellationToken ct);
    Task AddInteractionAsync(PostInteraction interaction, CancellationToken ct);
    Task IncrementShareCountAsync(int postId, CancellationToken ct);
    void RemoveInteraction(PostInteraction interaction);
}