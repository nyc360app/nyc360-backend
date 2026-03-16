using NYC360.Domain.Enums;

namespace NYC360.Domain.Dtos.Common;

public record VerificationDocDto(int Id, DocumentType Type, string FileUrl);