using NYC360.Domain.Enums;

namespace NYC360.API.Models.RssSources;

public record GetRssConnectionRequestsListRequest(
    int PageNumber = 1, 
    int PageSize = 10, 
    RssConnectionStatus? Status = null);
