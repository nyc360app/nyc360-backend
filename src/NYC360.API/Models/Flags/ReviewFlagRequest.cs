using NYC360.Domain.Enums;

namespace NYC360.API.Models.Flags;

public record ReviewFlagRequest(
    int FlagId,
    FlagStatus NewStatus,
    string? AdminNote
);