using NYC360.Domain.Entities.Communities;
using NYC360.Domain.Enums.Communities;

namespace NYC360.Domain.Dtos.Communities;

public record CommunityDisbandRequestDto(
    int Id,
    int CommunityId,
    string CommunityName,
    int RequestedByUserId,
    string RequestedByUserName,
    string Reason,
    DisbandRequestStatus Status,
    DateTime RequestedAt,
    DateTime? ProcessedAt,
    int? ProcessedByUserId,
    string? ProcessedByUserName,
    string? AdminNotes
);

public static class CommunityDisbandRequestDtoExtensions
{
    extension(CommunityDisbandRequestDto)
    {
        public static CommunityDisbandRequestDto Map(CommunityDisbandRequest entity)
        {
            var requestedByName = entity.RequestedByUser != null 
                ? $"{entity.RequestedByUser.FirstName} {entity.RequestedByUser.LastName}".Trim()
                : "Unknown";
                
            var processedByName = entity.ProcessedByUser != null 
                ? $"{entity.ProcessedByUser.FirstName} {entity.ProcessedByUser.LastName}".Trim()
                : null;
            
            return new CommunityDisbandRequestDto(
                entity.Id,
                entity.CommunityId,
                entity.Community?.Name ?? "Unknown",
                entity.RequestedByUserId,
                requestedByName,
                entity.Reason,
                entity.Status,
                entity.RequestedAt,
                entity.ProcessedAt,
                entity.ProcessedByUserId,
                processedByName,
                entity.AdminNotes
            );
        }
    }
}
