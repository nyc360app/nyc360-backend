using NYC360.Domain.Wrappers;

namespace NYC360.Domain.Dtos.Housing;

public record AgentDashboardDto(
    AgentDashboardStats Stats,
    List<AgentDashboardTrendPoint> Trends,
    List<AgentDashboardNeighborhoodStats> TopNeighborhoods,
    PagedResponse<AgentDashboardRecentInquiry> RecentInquiries
);

public record AgentDashboardStats(
    AgentDashboardStatItem TotalInquiries,
    AgentDashboardNewInquiries NewInquiriesToday,
    AgentDashboardTypeBreakdown TypeBreakdown
);

public record AgentDashboardStatItem(int Value, double MonthlyChangePercentage);

public record AgentDashboardNewInquiries(int Value, int? LastUpdatedMinutesAgo);

public record AgentDashboardTypeBreakdown(double RentPercentage, double SalePercentage);

public record AgentDashboardTrendPoint(DateTime Date, int Count);

public record AgentDashboardNeighborhoodStats(string Name, int Count);

public record AgentDashboardRecentInquiry(
    int Id,
    AgentDashboardUser User,
    AgentDashboardRequestType Type,
    string Neighborhood,
    string PropertyType,
    string Status
);

public enum AgentDashboardRequestType { Rent, Sale }

public record AgentDashboardUser(string Name, string Email);

public record AgentListingDto(
    int Id,
    string? ImageUrl,
    int Price,
    string Neighborhood,
    string HouseType,
    int Bedrooms,
    int Bathrooms,
    int TotalInquiries,
    bool IsPublished,
    DateTime CreatedAt,
    HouseListingAuthorizationDto? Authorization = null
);
