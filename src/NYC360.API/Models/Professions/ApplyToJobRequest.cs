namespace NYC360.API.Models.Professions;

public record ApplyToJobRequest(
    int JobOfferId,
    string? CoverLetter,
    IFormFile? Attachment
);