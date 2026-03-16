using NYC360.Domain.Enums;

namespace NYC360.API.Models.Post;

public record GetSavedPostsRequest(
    int Page = 1, 
    int PageSize = 10, 
    Category? Category = null
);
