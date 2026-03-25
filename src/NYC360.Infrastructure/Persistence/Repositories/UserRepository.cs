using NYC360.Infrastructure.Persistence.DbContexts;
using NYC360.Application.Contracts.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NYC360.Domain.Dtos.User;
using NYC360.Domain.Entities.User;
using NYC360.Domain.Enums;
using NYC360.Domain.Enums.Tags;

namespace NYC360.Infrastructure.Persistence.Repositories;

public class UserRepository(
    ApplicationDbContext db,
    UserManager<ApplicationUser> userManager)
    : IUserRepository
{
    public async Task<ApplicationUser?> GetAdminByIdAsync(int id, CancellationToken ct)
    {
        return await userManager.Users
            .Include(u => u.Profile)
            .FirstOrDefaultAsync(u => u.Id == id, ct);
    }

    public async Task<UserProfile?> GetByIdWithStatsAsync(int userId, CancellationToken ct)
    {
        return await db.UserProfiles
            .Include(p => p.Stats)
            .Include(u => u.User)
            .FirstOrDefaultAsync(u => u.UserId == userId, ct);
    }

    public async Task<UserProfile?> GetUserWithInterestsAsync(int userId, CancellationToken ct)
    {
        return await db.UserProfiles
            .Include(u => u.Interests)
            .FirstOrDefaultAsync(u => u.UserId == userId, ct);
    }

    public async Task<List<UserSearchResultDto>> SearchUsersAsync(string term, int limit, CancellationToken ct)
    {
        return await db.UserProfiles.AsNoTracking()
            .Include(u => u.Tags)!
                .ThenInclude(ut => ut.Tag)
            .Include(u => u.User)
            .Where(u => u.User!.UserName!.Contains(term) || u.FirstName.Contains(term) || u.LastName!.Contains(term))
            .Select(u => new UserSearchResultDto(
                u.UserId, u.User!.UserName!, $"{u.FirstName} {u.LastName}", u.AvatarUrl, 
                u.Tags!.FirstOrDefault(ut => ut.Tag!.Type == TagType.Identity)!.Tag!.Name))
            .Take(limit)
            .ToListAsync(ct);
    }

    public async Task<UserProfile?> GetProfileByUserIdAsync(int userId, CancellationToken ct)
    {
        return await db.UserProfiles
            .Include(up => up.Stats)
            .Include(up => up.User)
            .FirstOrDefaultAsync(up => up.UserId == userId, ct);
    }

    public async Task<UserProfile?> GetProfileInfoByUserIdAsync(int userId, CancellationToken ct)
    {
        return await db.UserProfiles
            .AsNoTracking()
            .Include(up => up.User)
            .Include(up => up.Stats)
            .Include(up => up.Interests)
            .Include(up => up.Tags)!.ThenInclude(ut => ut.Tag)
            .Include(up => up.SocialLinks)
            .Include(up => up.Positions)
            .Include(up => up.Educations)
            .Include(up => up.Address).ThenInclude(a => a.Location)
            .Include(u => u.BusinessInfo)
            .Include(u => u.VisitorInfo)
            .Include(u => u.OrganizationInfo).ThenInclude(oi => oi.Services)
            .Include(up => up.CommunityMemberships)
                .ThenInclude(cm => cm.Community)
            .FirstOrDefaultAsync(up => up.UserId == userId, ct);
    }
    public async Task<UserPosition?> GetPositionByIdAsync(int positionId, CancellationToken ct)
    {
        return await db.UserPositions
            .FirstOrDefaultAsync(p => p.Id == positionId, ct);
    }
    public async Task<UserEducation?> GetEducationByIdAsync(int educationId, CancellationToken ct)
    {
        return await db.UserEducations
            .FirstOrDefaultAsync(e => e.Id == educationId, ct);
    }
    public void RemovePosition(UserPosition position)
    {
        db.UserPositions.Remove(position);
    }

    public void RemoveEducation(UserEducation education)
    {
        db.UserEducations.Remove(education);
    }

    public async Task<bool> ExistsAsync(int userId, CancellationToken ct)
    {
        return await db.Users.AnyAsync(u => u.Id == userId, ct);
    }

    public async Task<ApplicationUser?> GetUserWithProfileByIdAsync(int id, CancellationToken ct)
    {
        return await db.Users
            .Include(u => u.Profile)
            .FirstOrDefaultAsync(u => u.Id == id, ct);
    }

    public async Task<UserProfile?> GetProfileAsync(string userName, CancellationToken ct)
    {
        return await db.UserProfiles.
            AsNoTracking()
            .Include(u => u.User)
            .Include(u => u.SocialLinks)
            .Include(u => u.Stats)
            .Include(u => u.Positions)
            .Include(u => u.Educations)
            .Include(u => u.Interests)
            .Include(u => u.Address).ThenInclude(a => a.Location)
            .Include(u => u.BusinessInfo)
            .Include(u => u.VisitorInfo)
            .Include(u => u.OrganizationInfo).ThenInclude(oi => oi.Services)
            .Include(u => u.Tags)!.ThenInclude(ut => ut.Tag)
            .Include(u => u.CommunityMemberships)
                .ThenInclude(cm => cm.Community)
            .FirstOrDefaultAsync(u => u.User!.UserName == userName.ToUpperInvariant(), ct);
    }
    public async Task<int> CountAsync(CancellationToken ct)
    { 
        return await userManager.Users.CountAsync(ct);
    }

    public async Task<List<ApplicationUser>> GetPagedUsersAsync(int page, int pageSize, CancellationToken ct)
    {
        return await userManager.Users
            .AsNoTracking()
            .Include(u => u.Profile)
            .OrderBy(u => u.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public async Task<IList<string>> GetUserRolesAsync(ApplicationUser user, CancellationToken ct)
    {
        return await userManager.GetRolesAsync(user);
    }
    
    public async Task<UserSocialLink?> GetSocialLinkByIdAsync(int linkId, CancellationToken ct)
    {
        return await db.UserSocialLinks
            .FirstOrDefaultAsync(s => s.Id == linkId, ct);
    }

    public void RemoveSocialLink(UserSocialLink link)
    {
        db.UserSocialLinks.Remove(link);
    }
}
