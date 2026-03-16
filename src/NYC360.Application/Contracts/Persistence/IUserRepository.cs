using NYC360.Domain.Entities.User;
using NYC360.Domain.Dtos.User;

namespace NYC360.Application.Contracts.Persistence;

public interface IUserRepository
{
    Task<UserProfile?> GetProfileAsync(string userName, CancellationToken ct);
    Task<int> CountAsync(CancellationToken ct);
    Task<List<ApplicationUser>> GetPagedUsersAsync(int page, int pageSize, CancellationToken ct);
    Task<IList<string>> GetUserRolesAsync(ApplicationUser user, CancellationToken ct);
    Task<UserProfile?> GetByIdWithStatsAsync(int userId, CancellationToken ct);
    Task<UserProfile?> GetUserWithInterestsAsync(int userId, CancellationToken ct);
    Task<List<UserSearchResultDto>> SearchUsersAsync(string term, int limit, CancellationToken ct);
    
    Task<UserProfile?> GetProfileByUserIdAsync(int userId, CancellationToken ct);
    
    Task<ApplicationUser?> GetUserWithProfileByIdAsync(int id, CancellationToken ct);
    Task<UserProfile?> GetProfileInfoByUserIdAsync(int userId, CancellationToken ct);
    Task<UserPosition?> GetPositionByIdAsync(int positionId, CancellationToken ct);
    Task<UserEducation?> GetEducationByIdAsync(int educationId, CancellationToken ct);
    void RemovePosition(UserPosition position);
    void RemoveEducation(UserEducation education);
    Task<bool> ExistsAsync(int userId, CancellationToken ct);

    Task<UserSocialLink?> GetSocialLinkByIdAsync(int linkId, CancellationToken ct);
    void RemoveSocialLink(UserSocialLink link);
}