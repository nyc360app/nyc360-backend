using NYC360.Domain.Dtos.Communities;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Communities.Queries.GetHome;

public record GetCommunityHomeQuery(
    int UserId,
    int Page,
    int PageSize
) : IRequest<StandardResponse<CommunityHomeDto>>;