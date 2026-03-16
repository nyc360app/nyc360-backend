using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NYC360.Domain.Entities.User;

public class NewYorkerInfo
{
    [Key, ForeignKey(nameof(UserProfile))]
    public int UserId { get; set; }
    
    public bool IsInterestedInVolunteering { get; set; }
    public bool IsOpenToAttendingLocalEvents { get; set; }
    public bool FollowNeighborhoodUpdates { get; set; }
    public bool MakeProfilePublic { get; set; }
    public bool DisplayNeighborhood { get; set; }
    public bool AllowMessagesFromVerifiedOrganizations { get; set; }
    
    public UserProfile? UserProfile { get; set; }
}