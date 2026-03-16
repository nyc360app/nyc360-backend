namespace NYC360.API.Models.Communities;

public record DisbandCommunityRequest(
    int CommunityId,
    string Reason
);
