using System.ComponentModel.DataAnnotations;
using NYC360.Domain.Enums;

namespace NYC360.Domain.Entities.User;

public class UserInterest
{
    [Key]
    public int UserId { get; set; }
    public UserProfile? User { get; set; }
    
    public Category Category { get; set; }
}