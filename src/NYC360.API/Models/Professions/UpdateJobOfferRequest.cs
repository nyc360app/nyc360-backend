using NYC360.Domain.Dtos.Location;
using NYC360.Domain.Enums.Professions;

namespace NYC360.API.Models.Professions;

public record UpdateJobOfferRequest(
    int OfferId,
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
    AddressInputDto Address
);