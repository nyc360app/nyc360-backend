namespace NYC360.Application.Features.SpaceListings.Common;

public static class SpaceListingNormalizer
{
    public static string NormalizeName(string value)
        => value.Trim().ToLowerInvariant();

    public static string? NormalizeWebsite(string? value)
        => string.IsNullOrWhiteSpace(value) ? null : value.Trim().ToLowerInvariant();

    public static string? NormalizeEmail(string? value)
        => string.IsNullOrWhiteSpace(value) ? null : value.Trim().ToLowerInvariant();

    public static string? NormalizePhone(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        var digits = new string(value.Where(char.IsDigit).ToArray());
        return string.IsNullOrWhiteSpace(digits) ? null : digits;
    }

    public static string? BuildAddressLine(string? street, string? buildingNumber)
    {
        var streetValue = street?.Trim();
        var buildingValue = buildingNumber?.Trim();

        if (string.IsNullOrWhiteSpace(streetValue) && string.IsNullOrWhiteSpace(buildingValue))
            return null;

        if (string.IsNullOrWhiteSpace(buildingValue))
            return streetValue;
        if (string.IsNullOrWhiteSpace(streetValue))
            return buildingValue;

        return $"{buildingValue} {streetValue}".Trim();
    }
}
