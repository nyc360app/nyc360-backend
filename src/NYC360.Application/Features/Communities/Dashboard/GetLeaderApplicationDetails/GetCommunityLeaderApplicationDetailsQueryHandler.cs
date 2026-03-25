using MediatR;
using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.Communities;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Communities.Dashboard.GetLeaderApplicationDetails;

public class GetCommunityLeaderApplicationDetailsQueryHandler(ICommunityRepository communityRepository)
    : IRequestHandler<GetCommunityLeaderApplicationDetailsQuery, StandardResponse<CommunityLeaderApplicationAdminDetailsDto>>
{
    public async Task<StandardResponse<CommunityLeaderApplicationAdminDetailsDto>> Handle(
        GetCommunityLeaderApplicationDetailsQuery request,
        CancellationToken ct)
    {
        var application = await communityRepository.GetLeaderApplicationByIdAsync(request.ApplicationId, ct);
        if (application == null)
        {
            return StandardResponse<CommunityLeaderApplicationAdminDetailsDto>.Failure(
                new ApiError("communityLeaderApplication.notFound", "Community leader application not found."));
        }

        return StandardResponse<CommunityLeaderApplicationAdminDetailsDto>.Success(
            CommunityLeaderApplicationAdminDtoExtensions.MapDetails(application));
    }
}
