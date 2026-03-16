using NYC360.Domain.Entities.User;
using NYC360.Domain.Enums.Communities;

namespace NYC360.Domain.Entities.Communities;

public class CommunityDisbandRequest
{
    public int Id { get; set; }
    public int CommunityId { get; set; }
    public int RequestedByUserId { get; set; }
    public string Reason { get; set; } = string.Empty;
    public DisbandRequestStatus Status { get; set; } = DisbandRequestStatus.Pending;
    public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ProcessedAt { get; set; }
    public int? ProcessedByUserId { get; set; }
    public string? AdminNotes { get; set; }
    
    // Navigation properties
    public Community Community { get; set; }
    public UserProfile? RequestedByUser { get; set; }
    public UserProfile? ProcessedByUser { get; set; }
}
