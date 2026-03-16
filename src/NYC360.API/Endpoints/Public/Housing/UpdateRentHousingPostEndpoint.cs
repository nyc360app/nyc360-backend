using NYC360.Application.Features.Housing.Commands.UpdateRent;
using NYC360.API.Models.Housing;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.Housing;

public class UpdateRentHousingPostEndpoint(IMediator mediator) : Endpoint<UpdateRentHousingRequest, StandardResponse>
{
    public override void Configure()
    {
        Put("/housing/{Id}/edit/rent");
        AllowFileUploads();
    }

    public override async Task HandleAsync(UpdateRentHousingRequest request, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        // Map request model to command, including the ID from the route
        var command = new UpdateRentHouseListingCommand(
            userId.Value,
            request.Id,
            request.HouseType,
            request.MoveInDate,
            request.MoveOutDate,
            request.PropertyType,
            request.Borough,
            request.ZipCode,
            request.MaxOccupants,
            request.Neighborhood,
            request.FullAddress,
            request.UnitNumber,
            request.GoogleMap,
            request.NearbyTransportation,
            request.Bedrooms,
            request.Bathrooms,
            request.MonthlyRent,
            request.SecurityDeposit,
            request.BrokerFee,
            request.MonthlyCostRange,
            request.BuildingType,
            request.BuiltIn,
            request.RenovatedIn,
            request.Sqft,
            request.FloorLevel,
            request.Heating,
            request.Cooling,
            request.TemperatureControl,
            request.Laundry,
            request.Amenities,
            request.ShortTermStayAllowed,
            request.ShortStayEligiblity,
            request.Furnished,
            request.AcceptsHousingVouchers,
            request.FamilyAndKidsFriendly,
            request.PetsFriendly,
            request.AccessibilityFriendly,
            request.SmokingAllowed,
            request.AcceptedHousingPrograms,
            request.Description,
            request.LeaseType,
            request.PrivacyType,
            request.SharedBathroomType,
            request.SharedKitchenType,
            request.AboutCurrentResident,
            request.UnitRulesAndPolicies,
            request.RoommatesGroupChat,
            request.AddDirectApplyLink,
            request.NewPhotos,
            request.DeletedPhotoIds,
            request.CoListing,
            request.AllowColisterEditing,
            request.IsPublished
        );
        
        var result = await mediator.Send(command, ct);
        await Send.OkAsync(result, ct);
    }
}