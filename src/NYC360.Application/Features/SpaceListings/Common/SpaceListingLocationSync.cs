using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.Location;
using NYC360.Domain.Entities.SpaceListings;

namespace NYC360.Application.Features.SpaceListings.Common;

public static class SpaceListingLocationSync
{
    public static async Task EnsureLocationLinkedAsync(
        SpaceListing listing,
        ILocationRepository locationRepository,
        CancellationToken ct)
    {
        if (listing.LocationId.HasValue)
            return;

        var borough = Clean(listing.Borough);
        var neighborhood = Clean(listing.Neighborhood);
        var locationName = Clean(listing.LocationName);
        var zipCode = ParseZipCode(listing.ZipCode);

        // Nothing useful to create/link.
        if (string.IsNullOrWhiteSpace(borough) &&
            string.IsNullOrWhiteSpace(neighborhood) &&
            string.IsNullOrWhiteSpace(locationName) &&
            zipCode == 0)
        {
            return;
        }

        var neighborhoodNet = FirstNonEmpty(locationName, neighborhood) ?? string.Empty;
        var finalNeighborhood = FirstNonEmpty(neighborhood, locationName) ?? string.Empty;
        var code = BuildCode(neighborhoodNet, borough);

        var location = await locationRepository.GetOrCreateLocationAsync(
            new LocationDto(
                null,
                borough,
                code,
                neighborhoodNet,
                finalNeighborhood,
                zipCode),
            ct);

        if (location != null)
            listing.LocationId = location.Id;
    }

    private static string? Clean(string? value)
        => string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private static int ParseZipCode(string? zipCode)
    {
        if (string.IsNullOrWhiteSpace(zipCode))
            return 0;

        var digits = new string(zipCode.Where(char.IsDigit).Take(5).ToArray());
        return int.TryParse(digits, out var parsed) ? parsed : 0;
    }

    private static string BuildCode(string? neighborhoodNet, string? borough)
    {
        var source = FirstNonEmpty(neighborhoodNet, borough);
        if (string.IsNullOrWhiteSpace(source))
            return string.Empty;

        var normalized = source.Trim().ToUpperInvariant();
        var chars = normalized
            .Where(c => char.IsLetterOrDigit(c) || c == '-' || c == '_')
            .Take(32)
            .ToArray();

        return new string(chars);
    }

    private static string? FirstNonEmpty(params string?[] values)
        => values.FirstOrDefault(v => !string.IsNullOrWhiteSpace(v));
}
