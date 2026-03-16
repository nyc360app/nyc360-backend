namespace NYC360.API.Models.Locations;

public record GetLocationsListRequest(int Page = 1, int PageSize = 10, string? Search = null);
