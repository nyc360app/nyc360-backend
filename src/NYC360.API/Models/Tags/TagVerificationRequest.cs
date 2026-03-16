using NYC360.Domain.Enums;

namespace NYC360.API.Models.Tags;

public record TagVerificationRequest(
    int TagId,
    string Reason,
    DocumentType DocumentType,
    IFormFile File
);