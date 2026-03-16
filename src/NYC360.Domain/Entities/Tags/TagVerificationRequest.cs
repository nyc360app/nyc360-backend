using System.ComponentModel.DataAnnotations;
using NYC360.Domain.Entities.User;
using NYC360.Domain.Enums;

namespace NYC360.Domain.Entities.Tags;

public class TagVerificationRequest
{
    [Key]
    public int Id { get; set; }
    public int UserId { get; set; }
    public UserProfile? User { get; set; }
    
    public int TargetTagId { get; set; }
    public Tag? TargetTag { get; set; }
    
    public string Reason { get; set; } // "Why do I want this tag?"
    public VerificationStatus Status { get; set; } = VerificationStatus.Pending;
    
    public string? AdminComment { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ProcessedAt { get; set; }
    
    public ICollection<VerificationDocument> Documents { get; set; } = new List<VerificationDocument>();
}