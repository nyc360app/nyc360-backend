namespace NYC360.Domain.Dtos.Communities;

public record AssignLeaderRequestDto(
    int CommunityId,
    int UserId
);
