using NYC360.Domain.Entities.User;

namespace NYC360.Domain.Entities.Communities;

public class CommunityJoinRequest
{
    public int Id { get; set; }
    public int CommunityId { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public Community? Community { get; set; }
    public UserProfile? User { get; set; }
}