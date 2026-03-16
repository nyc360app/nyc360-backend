using NYC360.Domain.Enums.Communities;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Models.Communities;

public record GetDiscoveryRequest(
    string? Search, 
    CommunityType? Type, 
    int? LocationId
) : PagedRequest;