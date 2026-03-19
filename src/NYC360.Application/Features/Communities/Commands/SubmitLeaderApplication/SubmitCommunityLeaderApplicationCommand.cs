using MediatR;
using Microsoft.AspNetCore.Http;
using NYC360.Domain.Dtos.Communities;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Communities.Commands.SubmitLeaderApplication;

public record SubmitCommunityLeaderApplicationCommand(
    int UserId,
    string FullName,
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
    IFormFile? VerificationFile
) : IRequest<StandardResponse<CommunityLeaderApplicationSubmissionDto>>;
