namespace NYC360.API.Models.Housing;

public record GetAdminHousingListRequest(
    int PageNumber = 1,
    int PageSize = 10,
    bool? IsPublished = null,
    string? Search = null
);
