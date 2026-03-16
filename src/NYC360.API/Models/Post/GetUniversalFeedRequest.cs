using NYC360.Domain.Enums.Posts;
using NYC360.Domain.Wrappers;
using NYC360.Domain.Enums;

namespace NYC360.API.Models.Post;

public record GetUniversalFeedRequest(
    Category? Category,
    int? LocationId,
    string? Search,
    PostType? Type
) : PagedRequest;