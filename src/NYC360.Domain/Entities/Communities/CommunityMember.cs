using NYC360.Domain.Entities.User;
using NYC360.Domain.Enums.Communities;

namespace NYC360.Domain.Entities.Communities;

public class CommunityMember
{
    public int Id { get; set; }
    public int CommunityId { get; set; }
    public int UserId { get; set; }
    public CommunityRole Role { get; set; } = CommunityRole.Member;
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    
    public Community? Community { get; set; } = null!;
    public UserProfile? User { get; set; } = null!;
}