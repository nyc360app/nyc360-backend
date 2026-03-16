using NYC360.Domain.Dtos.Posts;
using NYC360.Domain.Wrappers;
using NYC360.Domain.Enums;
using MediatR;

namespace NYC360.Application.Features.Posts.Queries.GetSavedPosts;

public record GetSavedPostsQuery(
    int UserId, 
    int Page = 1, 
    int PageSize = 10,
    Category? Category = null
) : IRequest<PagedResponse<PostDto>>;