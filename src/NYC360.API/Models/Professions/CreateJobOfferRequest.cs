using NYC360.Domain.Enums.Professions;
using NYC360.Domain.Dtos.Location;

namespace NYC360.API.Models.Professions;

public record CreateJobOfferRequest(
    string Title,
    string Description,
    string? Requirements,
    string? Benefits,
    string? Responsibilities,
    decimal? SalaryMin,
    decimal? SalaryMax,
    WorkArrangement WorkArrangement,
    EmploymentType EmploymentType,
    EmploymentLevel EmploymentLevel,
    AddressInputDto? Address
);