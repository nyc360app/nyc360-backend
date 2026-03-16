using NYC360.Infrastructure.Persistence.DbContexts;
using NYC360.Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;
using NYC360.Domain.Entities.User;

namespace NYC360.Infrastructure.Persistence.Repositories;

public class UserSavedPostRepository(ApplicationDbContext context) : IUserSavedPostRepository
{
    public async Task<bool> ExistsByUserAndPostIdAsync(int userId, int postId, CancellationToken ct)
    {
        return await context.UserSavedPosts
            .AnyAsync(x => x.UserId == userId && x.PostId == postId, ct);
    }

    public async Task AddAsync(UserSavedPost savedPost, CancellationToken ct)
    {
        await context.UserSavedPosts.AddAsync(savedPost, ct);
    }
    
    public async Task<UserSavedPost?> GetByUserAndPostIdAsync(int userId, int postId, CancellationToken ct)
    {
        return await context.UserSavedPosts
            .FirstOrDefaultAsync(x => x.UserId == userId && x.PostId == postId, ct);
    }

    public void Remove(UserSavedPost savedPost)
    {
        context.UserSavedPosts.Remove(savedPost);
    }
}