namespace NYC360.API.Models.Communities;

public record RemoveMemberRequest(int CommunityId, int TargetUserId);