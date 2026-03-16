namespace NYC360.API.Models.Professions;

public record ShareJobOfferRequest(
    int OfferId,
    string Content, // The social "pitch" for the job
    List<string>? Tags,
    int? CommunityId // Optional: if posting inside a specific Division 1 Community
);