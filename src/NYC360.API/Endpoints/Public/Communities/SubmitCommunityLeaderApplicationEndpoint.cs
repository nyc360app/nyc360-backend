using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using MediatR;
using NYC360.API.Extensions;
using NYC360.API.Models.Communities;
using NYC360.Application.Contracts.Persistence;
using NYC360.Application.Features.Communities.Commands.SubmitLeaderApplication;
using NYC360.Domain.Entities.User;
using NYC360.Domain.Dtos.Communities;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Public.Communities;

public class SubmitCommunityLeaderApplicationEndpoint(
    IMediator mediator,
    IUserRepository userRepository,
    UserManager<ApplicationUser> userManager)
    : Endpoint<SubmitCommunityLeaderApplicationRequest, StandardResponse<CommunityLeaderApplicationSubmissionDto>>
{
    public override void Configure()
    {
        Post("/communities/leader-applications/submit");
        Roles("Resident", "Organization", "Business", "Admin", "SuperAdmin");
        AllowFileUploads();
        Summary(s =>
        {
            s.Description = "Submit an application to become a community leader.";
            s.RequestParam(r => r.fullName, "Applicant full name.");
            s.RequestParam(r => r.email, "Applicant email address.");
            s.RequestParam(r => r.phoneNumber, "Applicant phone number.");
            s.RequestParam(r => r.communityName, "Community the applicant wants to lead.");
            s.RequestParam(r => r.location, "Area or location for the community.");
            s.RequestParam(r => r.profileLink, "Optional public profile or portfolio link.");
            s.RequestParam(r => r.motivation, "Why the applicant wants to become a community leader.");
            s.RequestParam(r => r.experience, "Relevant leadership or organizing experience.");
            s.RequestParam(r => r.ledBefore, "Whether the applicant has led a community before.");
            s.RequestParam(r => r.weeklyAvailability, "Weekly availability for community leadership.");
            s.RequestParam(r => r.agreedToGuidelines, "Must be true to submit.");
            s.RequestParam(r => r.verificationFile, "Supporting verification document (max 5 MB).");
        });
    }

    public override async Task HandleAsync(SubmitCommunityLeaderApplicationRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        if (!await HasGate1EligibilityAsync(userId.Value, ct))
        {
            await Send.OkAsync(StandardResponse<CommunityLeaderApplicationSubmissionDto>.Failure(
                new ApiError("community.gate1.notEligible", "Only verified Resident, Organization, or Business users can apply. Staff accounts are allowed.")), ct);
            return;
        }

        var command = new SubmitCommunityLeaderApplicationCommand(
            userId.Value,
            req.fullName,
            req.email,
            req.phoneNumber,
            req.communityName,
            req.location,
            req.profileLink,
            req.motivation,
            req.experience,
            req.ledBefore,
            req.weeklyAvailability,
            req.agreedToGuidelines,
            req.verificationFile);

        var result = await mediator.Send(command, ct);
        await Send.OkAsync(result, ct);
    }

    private async Task<bool> HasGate1EligibilityAsync(int userId, CancellationToken ct)
    {
        var profile = await userRepository.GetProfileByUserIdAsync(userId, ct);
        if (profile?.User == null)
            return false;

        var roles = await userManager.GetRolesAsync(profile.User);
        var isStaff = roles.Contains("SuperAdmin") || roles.Contains("Admin");
        if (isStaff)
            return true;

        var isAllowedRole = roles.Contains("Resident") || roles.Contains("Organization") || roles.Contains("Business");
        var isVerified = profile.Stats?.IsVerified ?? false;

        return isAllowedRole && isVerified;
    }
}
