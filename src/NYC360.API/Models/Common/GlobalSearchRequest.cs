using NYC360.Domain.Enums;

namespace NYC360.API.Models.Common;

public record GlobalSearchRequest(
    string Term,
    Category? Division,
    int Limit = 5
);