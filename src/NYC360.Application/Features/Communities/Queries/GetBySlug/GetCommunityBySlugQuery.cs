using NYC360.Domain.Wrappers;
using MediatR;
using NYC360.Domain.Dtos.Communities;

namespace NYC360.Application.Features.Communities.Queries.GetBySlug;

public record GetCommunityBySlugQuery(
    int UserId, 
    string Slug,
    int Page,
    int PageSize
) : IRequest<StandardResponse<CommunityHomePageDto>>;