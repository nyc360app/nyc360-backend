namespace NYC360.Domain.Dtos.Communities;

public record CommunityPendingRequestDto(
    int UserId,
    string UserName,
    string? UserAvatar,
    DateTime RequestedAt
);