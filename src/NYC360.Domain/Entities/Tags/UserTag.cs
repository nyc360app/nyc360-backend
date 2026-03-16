using NYC360.Domain.Entities.User;

namespace NYC360.Domain.Entities.Tags;

public class UserTag
{
    public int UserId { get; set; }
    public UserProfile? User { get; set; }
    
    public int TagId { get; set; }
    public Tag? Tag { get; set; }
}