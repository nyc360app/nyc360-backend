using NYC360.Domain.Entities.Professions;
using NYC360.Domain.Enums.Professions;

namespace NYC360.Domain.Dtos.Professions;

public record JobOfferMinimalDto(
    int Id,
    string Title,
    decimal? SalaryMin,
    decimal? SalaryMax,
    WorkArrangement WorkArrangement,
    EmploymentType EmploymentType,
    EmploymentLevel EmploymentLevel,
    string? CompanyName,
    string? AuthorAvatarUrl,
    string? AuthorUsername
);

public static class JobOfferMinimalDtoExtensions
{
    extension(JobOfferMinimalDto)
    {
        public static JobOfferMinimalDto Map(JobOffer offer) => new(
            offer.Id, 
            offer.Title, 
            offer.SalaryMin, 
            offer.SalaryMax, 
            offer.WorkArrangement, 
            offer.EmploymentType, 
            offer.EmploymentLevel,
            offer.Author != null ? (string.IsNullOrEmpty(offer.CompanyName) ? offer.Author.GetFullName() : offer.CompanyName) : offer.CompanyName,
            offer.Author?.AvatarUrl,
            offer.Author != null ? offer.Author.User!.UserName : null
        );
    }
}