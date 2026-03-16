using NYC360.Domain.Entities.Professions;
using NYC360.Domain.Enums.Professions;
using NYC360.Domain.Dtos.Location;
using NYC360.Domain.Dtos.User;

namespace NYC360.Domain.Dtos.Professions;

public record JobOfferDetailsDto(
    int Id,
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
    AddressDto Address,
    UserMinimalInfoDto Author,
    DateTime CreatedAt,
    bool IsApplied,
    int ApplicationsCount
);

public static class JobOfferDetailsDtoExtensions
{
    extension(JobOfferDetailsDto)
    {
        public static JobOfferDetailsDto Map(JobOffer offer, int applicationsCount, bool isApplied) => new JobOfferDetailsDto(
            offer.Id,
            offer.Title,
            offer.Description,
            offer.Requirements,
            offer.Benefits,
            offer.Responsablities,
            offer.SalaryMin,
            offer.SalaryMax,
            offer.WorkArrangement,
            offer.EmploymentType,
            offer.EmploymentLevel,
            AddressDto.Map(offer.Address!)!,
            UserMinimalInfoDto.Map(offer.Author!),
            offer.CreatedAt,
            isApplied,
            applicationsCount
        );
    }
}