using NYC360.Domain.Entities.Communities;
using NYC360.Domain.Enums.Communities;

namespace NYC360.Domain.Dtos.Communities;

public record CommunityLeaderApplicationAdminListItemDto(
    int ApplicationId,
    int UserId,
    string ApplicantName,
    string ApplicantUsername,
    string Email,
    string CommunityName,
    string Location,
    bool LedBefore,
    CommunityLeaderApplicationStatus Status,
    DateTime SubmittedAt
);

public record CommunityLeaderApplicationAdminDetailsDto(
    int ApplicationId,
    int UserId,
    string ApplicantName,
    string ApplicantUsername,
    string Email,
    string PhoneNumber,
    string CommunityName,
    string Location,
    string? ProfileLink,
    string Motivation,
    string Experience,
    bool LedBefore,
    string WeeklyAvailability,
    bool AgreedToGuidelines,
    string VerificationFileUrl,
    CommunityLeaderApplicationStatus Status,
    string? AdminNotes,
    DateTime SubmittedAt,
    DateTime? ReviewedAt
);

public static class CommunityLeaderApplicationAdminDtoExtensions
{
    extension(CommunityLeaderApplicationAdminListItemDto)
    {
        public static CommunityLeaderApplicationAdminListItemDto MapListItem(CommunityLeaderApplication entity)
        {
            return new CommunityLeaderApplicationAdminListItemDto(
                entity.Id,
                entity.UserId,
                entity.FullName,
                entity.User?.User?.UserName ?? string.Empty,
                entity.Email,
                entity.CommunityName,
                entity.Location,
                entity.LedBefore,
                entity.Status,
                entity.CreatedAt
            );
        }
    }

    extension(CommunityLeaderApplicationAdminDetailsDto)
    {
        public static CommunityLeaderApplicationAdminDetailsDto MapDetails(CommunityLeaderApplication entity)
        {
            return new CommunityLeaderApplicationAdminDetailsDto(
                entity.Id,
                entity.UserId,
                entity.FullName,
                entity.User?.User?.UserName ?? string.Empty,
                entity.Email,
                entity.PhoneNumber,
                entity.CommunityName,
                entity.Location,
                entity.ProfileLink,
                entity.Motivation,
                entity.Experience,
                entity.LedBefore,
                entity.WeeklyAvailability,
                entity.AgreedToGuidelines,
                entity.VerificationFileUrl,
                entity.Status,
                entity.AdminNotes,
                entity.CreatedAt,
                entity.ReviewedAt
            );
        }
    }
}
