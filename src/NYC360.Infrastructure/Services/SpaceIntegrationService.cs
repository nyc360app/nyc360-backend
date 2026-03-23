using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using NYC360.Application.Common.Settings;
using NYC360.Application.Contracts.Infrastructure;

namespace NYC360.Infrastructure.Services;

public class SpaceIntegrationService(HttpClient httpClient, IConfiguration configuration) : ISpaceIntegrationService
{
    public async Task<SpaceIntegrationResult> CreateListingAsync(SpaceIntegrationRequest request, CancellationToken ct)
    {
        var settings = configuration.GetSection("Space").Get<SpaceSettings>() ?? new SpaceSettings();
        if (!settings.Enabled || string.IsNullOrWhiteSpace(settings.BaseUrl))
        {
            return new SpaceIntegrationResult(false, null, null, null, "Space integration is disabled.");
        }

        var baseUri = settings.BaseUrl.TrimEnd('/');
        httpClient.BaseAddress = new Uri(baseUri);
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", settings.ApiKey);
        httpClient.DefaultRequestHeaders.Remove("Idempotency-Key");
        httpClient.DefaultRequestHeaders.Add("Idempotency-Key", request.IdempotencyKey);

        var payload = new
        {
            entityType = request.EntityType.ToString().ToLowerInvariant(),
            request.Name,
            request.Description,
            request.Website,
            request.PhoneNumber,
            request.PublicEmail,
            request.ContactName,
            address = new
            {
                request.AddressLine,
                request.Borough,
                request.Neighborhood,
                request.ZipCode
            },
            tags = request.Tags
        };

        try
        {
            var response = await httpClient.PostAsJsonAsync("/api/listings", payload, ct);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync(ct);
                return new SpaceIntegrationResult(false, null, null, null, error);
            }

            var result = await response.Content.ReadFromJsonAsync<SpaceCreateResponse>(cancellationToken: ct);
            if (result == null || string.IsNullOrWhiteSpace(result.SpaceItemId))
            {
                return new SpaceIntegrationResult(false, null, null, null, "Space response missing item id.");
            }

            return new SpaceIntegrationResult(true, result.SpaceItemId, result.EntityType, result.Slug, null);
        }
        catch (Exception ex)
        {
            return new SpaceIntegrationResult(false, null, null, null, ex.Message);
        }
    }

    private sealed record SpaceCreateResponse(string SpaceItemId, string? EntityType, string? Slug);
}
