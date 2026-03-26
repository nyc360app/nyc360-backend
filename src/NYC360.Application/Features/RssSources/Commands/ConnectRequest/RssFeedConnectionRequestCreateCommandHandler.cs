using NYC360.Application.Contracts.Persistence;
using NYC360.Application.Contracts.Services;
using NYC360.Application.Contracts.Storage;
using NYC360.Domain.Constants;
using NYC360.Domain.Entities;
using NYC360.Domain.Wrappers;
using NYC360.Domain.Enums;
using MediatR;

namespace NYC360.Application.Features.RssSources.Commands.ConnectRequest;

public class RssFeedConnectionRequestCreateCommandHandler(
    IRssFeedConnectionRequestRepository requestRepo,
    IRssSourceRepository rssSourceRepo,
    INewsAuthorizationService newsAuthorizationService,
    IUserRepository userRepository,
    ITagRepository tagRepository,
    ILocalStorageService localStorageService)
    : IRequestHandler<RssFeedConnectionRequestCreateCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(RssFeedConnectionRequestCreateCommand request, CancellationToken cancellationToken)
    {
        var normalizedUrl = request.Url.Trim();
        var normalizedName = request.Name.Trim();
        var normalizedDescription = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim();
        var normalizedImageUrl = string.IsNullOrWhiteSpace(request.ImageUrl) ? null : request.ImageUrl.Trim();
        var normalizedLanguage = string.IsNullOrWhiteSpace(request.Language) ? null : request.Language.Trim();
        var normalizedSourceWebsite = string.IsNullOrWhiteSpace(request.SourceWebsite) ? null : request.SourceWebsite.Trim();
        var normalizedSourceCredibility = string.IsNullOrWhiteSpace(request.SourceCredibility) ? null : request.SourceCredibility.Trim();
        var normalizedDivisionTag = string.IsNullOrWhiteSpace(request.DivisionTag) ? null : request.DivisionTag.Trim();
        var logoImageUrl = request.LogoImage is not null
            ? await localStorageService.SaveFileAsync(request.LogoImage, "rss-connect-logos", cancellationToken)
            : null;

        if (request.Category == Category.News)
        {
            var access = await newsAuthorizationService.GetAccessAsync(request.RequesterId, cancellationToken);
            if (access == null || !access.CanConnectRss)
                return StandardResponse.Failure(new ApiError("news.rss_forbidden", "Only Publisher-level News users can connect News RSS feeds."));
        }
        else if (request.Category == Category.Community)
        {
            var profile = await userRepository.GetProfileInfoByUserIdAsync(request.RequesterId, cancellationToken);
            if (profile?.User == null)
                return StandardResponse.Failure(new ApiError("auth.notfound", "User not found."));

            var roles = await userRepository.GetUserRolesAsync(profile.User, cancellationToken);
            var isStaff = roles.Contains("SuperAdmin", StringComparer.OrdinalIgnoreCase)
                || roles.Contains("SuccessAdmin", StringComparer.OrdinalIgnoreCase)
                || roles.Contains("Admin", StringComparer.OrdinalIgnoreCase);

            var hasCommunityLeaderTag = await tagRepository.UserHasTagAsync(
                request.RequesterId,
                CommunityVerificationTags.ApplyForCommunityLeaderBadgesName,
                cancellationToken);
            var hasCommunityOrganizationTag = await tagRepository.UserHasTagAsync(
                request.RequesterId,
                CommunityVerificationTags.ListCommunityOrganizationInSpaceName,
                cancellationToken);

            if (!isStaff && !hasCommunityLeaderTag && !hasCommunityOrganizationTag)
            {
                return StandardResponse.Failure(new ApiError(
                    "community.rss.requiresEligibleContributor",
                    "Only approved Community Leaders or approved Community Organization contributors can connect Community RSS feeds."));
            }
        }

        var urlExists = await rssSourceRepo.ExistsAsync(normalizedUrl, cancellationToken);
        if (urlExists)
            return StandardResponse.Failure(new ApiError("rss.url_duplicate", "RSS URL already exists in sources."));

        var hasPendingRequest = await requestRepo.HasPendingRequestAsync(normalizedUrl, request.Category, cancellationToken);
        if (hasPendingRequest)
            return StandardResponse.Failure(new ApiError("rss.request_pending", "A pending connection request already exists for this RSS URL."));
        
        var entity = new RssFeedConnectionRequest
        {
            Url = normalizedUrl,
            Category = request.Category,
            Name = normalizedName,
            Description = normalizedDescription,
            ImageUrl = normalizedImageUrl,
            LogoImageUrl = logoImageUrl,
            Language = normalizedLanguage,
            SourceWebsite = normalizedSourceWebsite,
            SourceCredibility = normalizedSourceCredibility,
            AgreementAccepted = request.AgreementAccepted,
            DivisionTag = normalizedDivisionTag,
            RequesterId = request.RequesterId,
            CreatedAt = DateTime.UtcNow,
            Status = Domain.Enums.RssConnectionStatus.Pending
        };

        await requestRepo.AddAsync(entity, cancellationToken);
        await requestRepo.SaveChangesAsync(cancellationToken);

        return StandardResponse.Success();
    }
}
