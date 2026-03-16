namespace NYC360.API.Models.Communities;

public record TransferOwnershipRequest(
    int CommunityId,
    int NewOwnerId
);
