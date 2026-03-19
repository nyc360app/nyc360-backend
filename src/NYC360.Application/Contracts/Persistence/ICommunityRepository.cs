using NYC360.Domain.Entities.Communities;
using NYC360.Domain.Enums.Communities;
using NYC360.Domain.Dtos.Communities;

namespace NYC360.Application.Contracts.Persistence;

public interface ICommunityRepository
{
    Task AddAsync(Community community, CancellationToken ct);
    Task<Community?> GetByIdAsync(int id, CancellationToken ct);
    Task<Community?> GetBySlugAsync(string slug, CancellationToken ct);
    Task<List<Community>> GetAllPaginatedAsync(int page, int itemsPerPage, CancellationToken ct);
    Task<(List<Community>, int)> SearchCommunitiesAsync(int userId, string? searchTerm, CommunityType? type,
        int? locationId, int page, int pageSize, CancellationToken ct);

    Task<List<CommunityDiscoveryDto>> SearchCommunitiesAsync(string term, int limit, CancellationToken ct);
    Task<(List<Community>, int)> SearchUserCommunitiesAsync(int userId, string? searchTerm, CommunityType? type,
        int? locationId, int page, int pageSize, CancellationToken ct);
    Task<bool> IsNameAvailableAsync(string name, CancellationToken ct);
    Task<bool> ExistsAsync(int id, CancellationToken ct);
    Task<bool> SlugExistsAsync(string slug, CancellationToken ct);
    Task<CommunityMember?> GetMemberAsync(int communityId, int userId, CancellationToken ct);
    Task<int> GetMemberCountAsync(int communityId, CancellationToken ct);
    Task<bool> IsMemberAsync(int communityId, int userId, CancellationToken ct);
    Task AddMemberAsync(CommunityMember member, CancellationToken ct);
    void RemoveMember(CommunityMember member);
    Task<List<CommunityMember>> GetMembersPaginatedAsync(int communityId, int page, int itemsPerPage, CancellationToken ct);
    Task<(List<CommunityMember>, int)> SearchMembersAsync(int communityId, string searchTerm, int page, int pageSize, CancellationToken ct);
    void Update(Community community);
    void Remove(Community community);

    Task<List<int>> GetJoinedCommunityIdsAsync(int userId, CancellationToken ct);
    Task<List<Community>> GetSuggestionsAsync(int userId, int count, CancellationToken ct);
    
    Task<CommunityJoinRequest?> GetJoinRequestAsync(int communityId, int userId, CancellationToken ct);
    Task<List<CommunityJoinRequest>> GetPendingRequestsAsync(int communityId, CancellationToken ct);
    Task<List<CommunityJoinRequest>> GetUserJoinRequestsAsync(int userId, CancellationToken ct);
    Task AddJoinRequestAsync(CommunityJoinRequest request, CancellationToken ct);
    void RemoveJoinRequest(CommunityJoinRequest request);
    
    // Disband request methods
    Task AddDisbandRequestAsync(CommunityDisbandRequest request, CancellationToken ct);
    Task<CommunityDisbandRequest?> GetDisbandRequestByIdAsync(int id, CancellationToken ct);
    Task<CommunityDisbandRequest?> GetPendingDisbandRequestAsync(int communityId, CancellationToken ct);
    Task<List<CommunityDisbandRequest>> GetUserDisbandRequestsAsync(int userId, CancellationToken ct);
    Task<(List<CommunityDisbandRequest>, int)> GetDisbandRequestsPaginatedAsync(DisbandRequestStatus? status, int page, int pageSize, CancellationToken ct);
    Task<bool> HasPendingDisbandRequestAsync(int communityId, CancellationToken ct);
    void UpdateDisbandRequest(CommunityDisbandRequest request);
    Task AddLeaderApplicationAsync(CommunityLeaderApplication application, CancellationToken ct);
    Task<bool> HasPendingLeaderApplicationAsync(int userId, CancellationToken ct);
    
    // Admin dashboard methods
    Task<(List<Community>, int)> GetAllCommunitiesPaginatedAsync(string? searchTerm, CommunityType? type, int? locationId, bool? hasDisbandRequest, int page, int pageSize, CancellationToken ct);
    Task<List<CommunityMember>> GetLeadersAsync(int communityId, CancellationToken ct);
    Task<int> GetLeaderCountAsync(int communityId, CancellationToken ct);
    Task<int> GetModeratorCountAsync(int communityId, CancellationToken ct);
}
