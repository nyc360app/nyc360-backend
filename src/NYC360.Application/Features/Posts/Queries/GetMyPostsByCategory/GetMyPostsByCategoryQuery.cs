using NYC360.Domain.Dtos.Posts;
using NYC360.Domain.Enums;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Posts.Queries.GetMyPostsByCategory;

public record GetMyPostsByCategoryQuery(
    Category Category, 
    int Page = 1, 
    int PageSize = 10, 
    int UserId = 0 // Required for "My Posts"
) : IRequest<PagedResponse<PostDto>>;
