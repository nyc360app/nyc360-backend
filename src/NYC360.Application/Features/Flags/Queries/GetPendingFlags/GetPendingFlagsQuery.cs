using NYC360.Domain.Dtos.Posts;
using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Flags.Queries.GetPendingFlags;

public record GetPendingFlagsQuery(
    int Page,
    int PageSize
) : IRequest<PagedResponse<PostFlagAdminDto>>;