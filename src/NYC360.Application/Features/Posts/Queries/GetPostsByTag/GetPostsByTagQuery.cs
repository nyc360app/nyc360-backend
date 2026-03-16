using NYC360.Domain.Dtos.Posts;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Posts.Queries.GetPostsByTag;

public record GetPostsByTagQuery(
    string TagName, 
    int Page = 1, 
    int PageSize = 10, 
    int? UserId = null
) : IRequest<PagedResponse<PostDto>>;