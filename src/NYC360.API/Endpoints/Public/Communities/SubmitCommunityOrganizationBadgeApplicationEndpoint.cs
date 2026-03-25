using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using MediatR;
using NYC360.API.Extensions;
using NYC360.API.Models.Communities;
using NYC360.Application.Contracts.Persistence;
using NYC360.Application.Features.Verifications.Commands.TagRequest;
using NYC360.Domain.Constants;
using NYC360.Domain.Entities.User;
using NYC360.Domain.Enums;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Public.Communities;

public class SubmitCommunityOrganizationBadgeApplicationEndpoint(
    IMediator mediator,
    IUserRepository userRepository,
    IVerificationRepository verificationRepository,
    UserManager<ApplicationUser> userManager)
    : Endpoint<SubmitCommunityLeaderApplicationRequest, StandardResponse>
{
    public override void Configure()
    {
        Post("/communities/organization-listing-badge/submit");
        Roles("Resident", "NewYorker", "Organization", "Business", "Admin", "SuccessAdmin", "SuperAdmin");
        AllowFileUploads();
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
            await Send.OkAsync(StandardResponse.Failure(
                new ApiError("community.gate1.notEligible", "Only verified Resident, Organization, or Business users can apply. Staff accounts are allowed.")), ct);
            return;
        }

        if (!req.agreedToGuidelines)
        {
            await Send.OkAsync(StandardResponse.Failure(
                new ApiError("communityTagApplication.guidelines.required", "You must agree to the community guidelines before submitting.")), ct);
            return;
        }

        if (req.verificationFile is null)
        {
            await Send.OkAsync(StandardResponse.Failure(
                new ApiError("communityTagApplication.verificationFile.required", "Verification file is required.")), ct);
            return;
        }

        var reason = BuildReason("D01.3 - Community Organization Listing Badge", req);
        var command = new SubmitTagVerificationCommand(
            userId.Value,
            CommunityVerificationTags.ListCommunityOrganizationInSpaceId,
            reason,
            DocumentType.OrganizationCharter,
            req.verificationFile);

        var result = await mediator.Send(command, ct);
        await Send.OkAsync(result, ct);
    }

    private static string BuildReason(string applicationType, SubmitCommunityLeaderApplicationRequest req)
    {
        var reason = string.Join(" | ", new[]
        {
            applicationType,
            $"FullName: {Trim(req.fullName, 200)}",
            $"Email: {Trim(req.email, 255)}",
            $"Phone: {Trim(req.phoneNumber, 50)}",
            $"CommunityName: {Trim(req.communityName, 200)}",
            $"Location: {Trim(req.location, 200)}",
            $"ProfileLink: {Trim(req.profileLink, 300)}",
            $"LedBefore: {req.ledBefore}",
            $"WeeklyAvailability: {Trim(req.weeklyAvailability, 300)}",
            $"Motivation: {Trim(req.motivation, 1200)}",
            $"Experience: {Trim(req.experience, 1200)}"
        });

        return reason.Length <= 5000 ? reason : reason[..5000];
    }

    private static string Trim(string? value, int max)
    {
        if (string.IsNullOrWhiteSpace(value))
            return string.Empty;

        var normalized = value.Trim();
        return normalized.Length <= max ? normalized : normalized[..max];
    }

    private async Task<bool> HasGate1EligibilityAsync(int userId, CancellationToken ct)
    {
        var profile = await userRepository.GetProfileByUserIdAsync(userId, ct);
        if (profile?.User == null)
            return false;

        var roles = await userManager.GetRolesAsync(profile.User);
        var isStaff = roles.Contains("SuperAdmin") || roles.Contains("SuccessAdmin") || roles.Contains("Admin");
        if (isStaff)
            return true;

        var isAllowedRole = roles.Contains("Resident") || roles.Contains("NewYorker") || roles.Contains("Organization") || roles.Contains("Business");
        var isVerified = profile.Stats?.IsVerified ?? false;
        if (!isVerified)
            isVerified = await verificationRepository.HasApprovedIdentityRequestAsync(userId, ct);

        return isAllowedRole && isVerified;
    }
}
