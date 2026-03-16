using NYC360.Domain.Entities.User;

namespace NYC360.Application.Contracts.Persistence;

public interface IUserSavedPostRepository
{
    Task<bool> ExistsByUserAndPostIdAsync(int userId, int postId, CancellationToken ct);
    Task AddAsync(UserSavedPost savedPost, CancellationToken ct);
    Task<UserSavedPost?> GetByUserAndPostIdAsync(int userId, int postId, CancellationToken ct);
    void Remove(UserSavedPost savedPost);
}
