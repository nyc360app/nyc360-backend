namespace NYC360.Domain.Dtos.Professions;

public record JobOfferProfileDto(
    JobOfferDetailsDto Offer,
    List<JobOfferMinimalDto> RelatedJobs
);