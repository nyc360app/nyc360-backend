using NYC360.Domain.Enums.SpaceListings;

namespace NYC360.Application.Contracts.Infrastructure;

public interface ISpaceIntegrationService
{
    Task<SpaceIntegrationResult> CreateListingAsync(SpaceIntegrationRequest request, CancellationToken ct);
}

public record SpaceIntegrationRequest(
    string IdempotencyKey,
    SpaceListingEntityType EntityType,
    string Name,
    string Description,
    string? Website,
    string? PhoneNumber,
    string? PublicEmail,
    string? ContactName,
    string? AddressLine,
    string? Borough,
    string? Neighborhood,
    string? ZipCode,
    List<string> Tags
);

public record SpaceIntegrationResult(
    bool Success,
    string? SpaceItemId,
    string? SpaceEntityType,
    string? SpaceSlug,
    string? ErrorMessage,
    bool IsPublishSkipped = false
);
