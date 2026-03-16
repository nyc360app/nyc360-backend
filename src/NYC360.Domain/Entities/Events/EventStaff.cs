using NYC360.Domain.Entities.User;
using NYC360.Domain.Enums;
using NYC360.Domain.Enums.Events;

namespace NYC360.Domain.Entities.Events;

public class EventStaff
{
    public int EventId { get; set; }
    public Event Event { get; set; } = null!;

    public int UserId { get; set; }
    public UserProfile User { get; set; } = null!;

    // Role and Permissions
    public EventRole Role { get; set; }
    // 1. [Event] Organization Permissions (Sheet 3, Col 3)
    public bool HasFinancialAuthority { get; set; } // Full ownership & Withdrawals
    public bool CanManageTicketing { get; set; }    // Pricing, Tiers, Refund Policy

    // 2. [Event] Promoter Permissions (Sheet 3, Col 5)
    public bool CanManageCampaigns { get; set; }    // Ads, Promo Codes, Influencers
    public bool CanViewRevenueAttribution { get; set; } // Performance-based payouts visibility

    // 3. [Venue] Host Permissions (Sheet 3, Col 7)
    public bool HasApprovalAuthority { get; set; }  // Event approval authority
    public bool CanEnforceCompliance { get; set; } // Compliance validation & Capacity enforcement
    
    // 4. Global Scope (Sheet 2: Analytics Scope)
    public bool HasFullAnalyticsAccess { get; set; } // True for Admin, False for Promoters (Sales only)
    
    // Revenue
    public decimal? FixedFee { get; set; }
    public double? TicketPercentage { get; set; }
    public bool IsPerformanceBasedOnly { get; set; }
    public double? BarRevenuePercentage { get; set; }
}