using NYC360.Domain.Dtos.Posts;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Culture.Queries.GetStories;

public record GetCultureStoriesQuery(
    int UserId,
    string? Tag,
    string? TimeFrame,
    int? LocationId,
    int Page = 1, 
    int PageSize = 10
) : IRequest<PagedResponse<PostDto>>;