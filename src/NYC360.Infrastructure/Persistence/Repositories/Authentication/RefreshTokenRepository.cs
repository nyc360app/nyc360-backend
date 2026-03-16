using NYC360.Infrastructure.Persistence.DbContexts;
using NYC360.Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;
using NYC360.Domain.Entities;

namespace NYC360.Infrastructure.Persistence.Repositories.Authentication;

public class RefreshTokenRepository(ApplicationDbContext db) : IRefreshTokenRepository
{
    public async Task AddAsync(RefreshToken token, CancellationToken ct)
        => await db.RefreshTokens.AddAsync(token, ct);

    public async Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken ct)
        => await db.RefreshTokens.FirstOrDefaultAsync(t => t.Token == token, ct);

    public async Task RemoveAsync(int userId, CancellationToken ct)
    {
        var tokens = await db.RefreshTokens
            .Where(t => t.UserId == userId)
            .ToListAsync(ct);

        if (tokens.Count == 0)
            return;

        db.RefreshTokens.RemoveRange(tokens);
    }

    public async Task SaveChangesAsync(CancellationToken ct) 
        => await db.SaveChangesAsync(ct);
}