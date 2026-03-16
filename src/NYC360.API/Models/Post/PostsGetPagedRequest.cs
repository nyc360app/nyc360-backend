using NYC360.Domain.Enums;
using NYC360.Domain.Enums.Posts;

namespace NYC360.API.Models.Post;

public record PostsGetPagedRequest(
    int Page = 1,
    int PageSize = 10,
    Category? Category = null,
    string? Search = null,
    PostType? PostType = null,
    PostSource? SourceType = null
);
