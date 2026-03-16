namespace NYC360.API.Models.Post;

public record TrendingPostsGetRequest(
    int Page = 1,
    int PageSize = 10
);