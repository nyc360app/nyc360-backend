using NYC360.Domain.Enums;

namespace NYC360.API.Models.Users;

public record IdentityVerificationRequest(
    string Reason,
    DocumentType DocumentType,
    IFormFile File
);