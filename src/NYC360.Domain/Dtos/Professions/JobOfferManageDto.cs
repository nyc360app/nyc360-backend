namespace NYC360.Domain.Dtos.Professions;

public record JobOfferManageDto(
    int Id,
    string Title,
    decimal? SalaryMin,
    decimal? SalaryMax,
    bool IsActive,
    DateTime CreatedAt,
    int ApplicationCount
);