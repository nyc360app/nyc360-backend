using NYC360.Domain.Enums;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Models.Post;

public record GetPostsByCategoryRequest(Category Category) : PagedRequest;
