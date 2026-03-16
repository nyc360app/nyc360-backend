namespace NYC360.API.Models.Locations;

public record SearchLocationRequest(string? Query, int Limit = 20);