using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.Posts;
using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Flags.Queries.GetPendingFlags;

public class GetPendingFlagsQueryHandler(IPostFlagRepository postFlagRepository)
    : IRequestHandler<GetPendingFlagsQuery, PagedResponse<PostFlagAdminDto>>
{
    public async Task<PagedResponse<PostFlagAdminDto>> Handle(GetPendingFlagsQuery request, CancellationToken ct)
    {
        var (items, total) = await postFlagRepository.GetPendingFlagsPaginatedAsync(
            request.Page,
            request.PageSize,
            ct);

        return PagedResponse<PostFlagAdminDto>.Create(
            items,
            request.Page,
            request.PageSize,
            total
        );
    }
}