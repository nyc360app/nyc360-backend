using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using NYC360.API.Extensions;
using NYC360.API.Models.SpaceListings;
using NYC360.Domain.Dtos.Location;
using NYC360.Domain.Enums;
using NYC360.Domain.Enums.SpaceListings;
using NYC360.Domain.Wrappers;
using SubmitSpaceListingCommand = NYC360.Application.Features.SpaceListings.Commands.Submit.SubmitSpaceListingCommand;
using SubmitSpaceListingSocialLinkInput = NYC360.Application.Features.SpaceListings.Commands.Submit.SpaceListingSocialLinkInput;
using SubmitSpaceListingHourInput = NYC360.Application.Features.SpaceListings.Commands.Submit.SpaceListingHourInput;

namespace NYC360.API.Endpoints.Public.SpaceListings;

public class SubmitSpaceListingEndpoint(IMediator mediator, ILogger<SubmitSpaceListingEndpoint> logger)
    : Endpoint<SubmitSpaceListingRequest, StandardResponse<int>>
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

    public override void Configure()
    {
        Post("/space/listings/submit");
        Permissions(Domain.Constants.Permissions.SpaceListings.Submit);
        AllowFileUploads();
    }

    public override async Task HandleAsync(SubmitSpaceListingRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        if (!TryParseEnum(req.Department, out Category department))
        {
            await SendWithStatusAsync(
                StandardResponse<int>.Failure(new ApiError("space.listing.department.invalid", "Department is required and must be a valid value.")),
                StatusCodes.Status400BadRequest,
                ct);
            return;
        }

        if (!TryParseEnum(req.EntityType, out SpaceListingEntityType entityType))
        {
            await SendWithStatusAsync(
                StandardResponse<int>.Failure(new ApiError("space.listing.entity_type.invalid", "EntityType is required and must be a valid value.")),
                StatusCodes.Status400BadRequest,
                ct);
            return;
        }

        if (string.IsNullOrWhiteSpace(req.Name))
        {
            await SendWithStatusAsync(
                StandardResponse<int>.Failure(new ApiError("space.listing.name.required", "Name is required.")),
                StatusCodes.Status400BadRequest,
                ct);
            return;
        }

        if (string.IsNullOrWhiteSpace(req.Description))
        {
            await SendWithStatusAsync(
                StandardResponse<int>.Failure(new ApiError("space.listing.description.required", "Description is required.")),
                StatusCodes.Status400BadRequest,
                ct);
            return;
        }

        var address = req.Address ?? new AddressInputDto(
            req.AddressId,
            req.LocationId,
            req.Street,
            req.BuildingNumber,
            req.ZipCode);

        try
        {
            var categories = req.Categories ?? ParseJsonList<Category>(req.CategoriesJson);
            var tags = req.Tags ?? ParseJsonList<string>(req.TagsJson);
            var organizationServices = req.OrganizationServices ?? ParseJsonList<Domain.Enums.Users.OrganizationServices>(req.OrganizationServicesJson);
            var socialLinks = req.SocialLinks ?? ParseJsonList<NYC360.API.Models.SpaceListings.SpaceListingSocialLinkInput>(req.SocialLinksJson);
            var hours = req.Hours ?? ParseJsonList<NYC360.API.Models.SpaceListings.SpaceListingHourInput>(req.HoursJson);

            var command = new SubmitSpaceListingCommand(
                userId.Value,
                department,
                entityType,
                req.Name!,
                req.Description!,
                address,
                req.LocationName,
                req.Borough,
                req.Neighborhood,
                req.Website,
                req.PhoneNumber,
                req.PublicEmail,
                req.ContactName,
                req.SubmitterNote,
                req.IsClaimingOwnership,
                categories ?? [],
                tags ?? [],
                req.BusinessIndustry,
                req.BusinessSize,
                req.BusinessServiceArea,
                req.BusinessServices,
                req.BusinessOwnershipType,
                req.BusinessIsLicensedInNyc,
                req.BusinessIsInsured,
                req.OrganizationType,
                req.OrganizationFundType,
                organizationServices ?? [],
                req.OrganizationIsTaxExempt,
                req.OrganizationIsNysRegistered,
                socialLinks?.Select(x => new SubmitSpaceListingSocialLinkInput(x.Platform, x.Url)).ToList() ?? [],
                hours?.Select(x => new SubmitSpaceListingHourInput(x.DayOfWeek, x.OpenTime, x.CloseTime, x.IsClosed)).ToList() ?? [],
                req.SaveAsDraft,
                req.Images,
                req.Documents,
                req.OwnershipDocuments,
                req.ProofDocuments
            );

            var result = await mediator.Send(command, ct);
            if (!result.IsSuccess)
            {
                await SendWithStatusAsync(result, StatusCodes.Status400BadRequest, ct);
                return;
            }

            await Send.OkAsync(result, ct);
        }
        catch (JsonException ex)
        {
            logger.LogWarning(ex, "Invalid JSON payload for space listing submission.");
            await SendWithStatusAsync(
                StandardResponse<int>.Failure(new ApiError("space.listing.invalid_json", "One or more JSON form fields are invalid.")),
                StatusCodes.Status400BadRequest,
                ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled error during space listing submission.");
            await SendWithStatusAsync(
                StandardResponse<int>.Failure(new ApiError("space.listing.submit_failed", "Unable to process listing submission.")),
                StatusCodes.Status500InternalServerError,
                ct);
        }
    }

    private static bool TryParseEnum<TEnum>(string? input, out TEnum value)
        where TEnum : struct, Enum
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            value = default;
            return false;
        }

        if (Enum.TryParse<TEnum>(input, true, out var parsed) && Enum.IsDefined(parsed))
        {
            value = parsed;
            return true;
        }

        if (int.TryParse(input, out var numeric) && Enum.IsDefined(typeof(TEnum), numeric))
        {
            value = (TEnum)Enum.ToObject(typeof(TEnum), numeric);
            return true;
        }

        value = default;
        return false;
    }

    private static List<T>? ParseJsonList<T>(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return null;

        return JsonSerializer.Deserialize<List<T>>(json, JsonOptions);
    }

    private async Task SendWithStatusAsync(StandardResponse<int> response, int statusCode, CancellationToken ct)
    {
        HttpContext.Response.StatusCode = statusCode;
        await HttpContext.Response.WriteAsJsonAsync(response, cancellationToken: ct);
    }
}
