namespace NYC360.Domain.Dtos.Communities;

public record MyCommunityRequestsDto(
    List<MyCommunityJoinRequestDto> JoinRequests,
    List<CommunityDisbandRequestDto> DisbandRequests
);
