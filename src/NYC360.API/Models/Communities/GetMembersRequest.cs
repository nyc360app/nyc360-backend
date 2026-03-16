using NYC360.Domain.Wrappers;

namespace NYC360.API.Models.Communities;

public record GetMembersRequest(int CommunityId) : PagedRequest;