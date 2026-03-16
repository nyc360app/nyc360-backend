using NYC360.Domain.Entities.Communities;

namespace NYC360.Domain.Dtos.Communities;

public record CommunityMemberDto(
    int UserId,
    string Name,
    string? AvatarUrl,
    string Role, // "Owner", "Moderator", "Member"
    DateTime JoinedAt
);

public static class CommunityMemberDtoExtensions
{
    extension(CommunityMemberDto)
    {
        public static CommunityMemberDto Map(CommunityMember communityMember)
        {
            return new CommunityMemberDto(
                communityMember.UserId,
                communityMember.User!.GetFullName(),
                communityMember.User.AvatarUrl,
                communityMember.Role.ToString(),
                communityMember.JoinedAt
            );
        }
    }
}