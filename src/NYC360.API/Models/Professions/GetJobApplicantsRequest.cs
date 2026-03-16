using NYC360.Domain.Wrappers;

namespace NYC360.API.Models.Professions;

public record GetJobApplicantsRequest(int OfferId) : PagedRequest;