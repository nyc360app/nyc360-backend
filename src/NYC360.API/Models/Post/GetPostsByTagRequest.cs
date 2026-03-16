using NYC360.Domain.Wrappers;

namespace NYC360.API.Models.Post;

public record GetPostsByTagRequest(string Tag) : PagedRequest;