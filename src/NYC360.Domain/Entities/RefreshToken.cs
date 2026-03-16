using System.ComponentModel.DataAnnotations;
using NYC360.Domain.Entities.User;

namespace NYC360.Domain.Entities;

public class RefreshToken
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Token { get; set; } = null!;
    public int UserId { get; set; } 
    public bool IsRevoked { get; set; } = false;
    public DateTime ExpiresAt { get; set; }
    
    public ApplicationUser? User { get; set; }
}