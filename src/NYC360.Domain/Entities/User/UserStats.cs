using System.ComponentModel.DataAnnotations;

namespace NYC360.Domain.Entities.User;

public class UserStats
{
    [Key]
    public int UserId { get; set; }

    public int PostsCount { get; set; }
    public int FollowersCount { get; set; }
    public int FollowingCount { get; set; }

    public int CommentsCount { get; set; }
    public int LikesReceived { get; set; }
    public int Shares { get; set; }

    public bool? IsVerified { get; set; }

    public UserProfile? User { get; set; }
}