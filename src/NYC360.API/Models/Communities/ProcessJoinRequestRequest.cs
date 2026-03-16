namespace NYC360.API.Models.Communities;

public record ProcessJoinRequestRequest(
    int CommunityId, 
    int TargetUserId
);