using NYC360.Application.Features.Housing.Commands.CreateAuthorization;
using NYC360.API.Models.Housing;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.Housing;

public class CreateHouseListingAuthorizationEndpoint(IMediator mediator) : Endpoint<CreateHouseListingAuthorizationRequest, StandardResponse<int>>
{
    public override void Configure()
    {
        Post("/housing/create/authoring");
        Permissions(Domain.Constants.Permissions.Housing.Create);
        AllowFileUploads();
    }

    public override async Task HandleAsync(CreateHouseListingAuthorizationRequest request, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var command = new CreateHouseListingAuthorizationCommand(
            userId.Value,
            request.HouseListingId,
            request.FullName,
            request.OrganizationName,
            request.Email,
            request.PhoneNumber,
            ResolveAvailabilities(request).Select(x => new AvailabilitySlotDto(
                x.AvailabilityType,
                x.Dates ?? new List<DateOnly>(),
                x.TimeFrom,
                x.TimeTo
            )).ToList(),
            request.AuthorizationType,
            request.ListingAuthorizationDocument,
            request.AuthorizationValidationDate,
            request.SaveThisAuthorizationForFutureListings,
            request.Attachments
        );

        var result = await mediator.Send(command, ct);
        await Send.OkAsync(result, cancellation: ct);
    }

    private static List<AvailabilitySlotRequest> ResolveAvailabilities(CreateHouseListingAuthorizationRequest request)
    {
        if (!string.IsNullOrWhiteSpace(request.Availability))
        {
            try
            {
                var options = new System.Text.Json.JsonSerializerOptions 
                { 
                    PropertyNameCaseInsensitive = true 
                };
                var list = System.Text.Json.JsonSerializer.Deserialize<List<AvailabilitySlotRequest>>(request.Availability, options);
                return list ?? new List<AvailabilitySlotRequest>();
            }
            catch
            {
                // return empty if JSON fails
            }
        }
        return new List<AvailabilitySlotRequest>();
    }
}
