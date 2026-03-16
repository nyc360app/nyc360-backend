using NYC360.Infrastructure.Persistence.DbContexts;
using NYC360.Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;
using NYC360.Domain.Entities.Posts;

namespace NYC360.Infrastructure.Persistence.Repositories;

public sealed class PostInteractionRepository(ApplicationDbContext db) : IPostInteractionRepository
{
    public async Task<PostInteraction?> GetInteractionAsync(int postId, int userId, CancellationToken ct)
    {
        return await db.PostUserInteractions
            .FirstOrDefaultAsync(x => x.PostId == postId && x.UserId == userId, ct);
    }

    public async Task AddInteractionAsync(PostInteraction interaction, CancellationToken ct)
    {
        await db.PostUserInteractions.AddAsync(interaction, ct);
    }
    
    public async Task IncrementShareCountAsync(int postId, CancellationToken ct)
    {
        await db.PostStats
            .Where(s => s.PostId == postId)
            .ExecuteUpdateAsync(s => s.SetProperty(
                    p => p.Shares, 
                    p => p.Shares + 1), 
                ct);
    }
    
    public void RemoveInteraction(PostInteraction interaction)
    {
        db.PostUserInteractions.Remove(interaction);
    }
}