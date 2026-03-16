using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.Communities;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Communities.Queries.GetMyCommunityRequests;

public class GetMyCommunityRequestsQueryHandler(ICommunityRepository communityRepository)
    : IRequestHandler<GetMyCommunityRequestsQuery, StandardResponse<MyCommunityRequestsDto>>
{
    public async Task<StandardResponse<MyCommunityRequestsDto>> Handle(GetMyCommunityRequestsQuery request, CancellationToken ct)
    {
        var joinRequests = await communityRepository.GetUserJoinRequestsAsync(request.UserId, ct);
        var disbandRequests = await communityRepository.GetUserDisbandRequestsAsync(request.UserId, ct);

        var joinRequestDtos = joinRequests.Select(MyCommunityJoinRequestDto.Map).ToList();
        var disbandRequestDtos = disbandRequests.Select(CommunityDisbandRequestDto.Map).ToList();

        var result = new MyCommunityRequestsDto(joinRequestDtos, disbandRequestDtos);

        return StandardResponse<MyCommunityRequestsDto>.Success(result);
    }
}
