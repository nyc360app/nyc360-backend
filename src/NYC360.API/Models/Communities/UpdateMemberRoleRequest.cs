using NYC360.Domain.Enums.Communities;

namespace NYC360.API.Models.Communities;

public record UpdateMemberRoleRequest(
    int CommunityId,
    int TargetUserId,
    CommunityRole NewRole
);
