using NYC360.Domain.Enums;

namespace NYC360.API.Models.RssSources;

public record GetLatestRssFeedItemsRequest(
    Category Category,
    int Limit = 1);
