using NYC360.Domain.Wrappers;

namespace NYC360.API.Models.Housing;

public record GetHousingFeedRequest(
    bool? IsRenting = null,
    int? MinPrice = null,
    int? MaxPrice = null,
    int? LocationId = null,
    string? Search = null    
) : PagedRequest;