using NYC360.Domain.Enums;

namespace NYC360.API.Models.Homes;

public record GetCommonHomeRequest(Category? Division, int Limit = 5);