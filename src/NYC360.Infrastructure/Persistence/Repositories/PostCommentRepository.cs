using NYC360.Infrastructure.Persistence.DbContexts;
using NYC360.Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;
using NYC360.Domain.Entities.Posts;

namespace NYC360.Infrastructure.Persistence.Repositories;

public class PostCommentRepository(ApplicationDbContext db) : IPostCommentRepository
{
    public async Task<PostComment?> GetCommentByIdAsync(int commentId, CancellationToken ct)
    {
        return await db.PostComments
            .Include(c => c.User)!.ThenInclude(up => up!.User)
            .Include(c => c.Replies)
            .FirstOrDefaultAsync(c => c.Id == commentId, ct);
    }

    public async Task AddCommentAsync(PostComment comment, CancellationToken ct)
    {
        await db.PostComments.AddAsync(comment, ct);
    }
}