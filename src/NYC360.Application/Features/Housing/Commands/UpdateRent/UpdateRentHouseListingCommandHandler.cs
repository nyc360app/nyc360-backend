using NYC360.Application.Contracts.Persistence;
using NYC360.Application.Contracts.Storage;
using NYC360.Domain.Entities.Housing;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Housing.Commands.UpdateRent;

public class UpdateRentHouseListingCommandHandler(
    IHouseInfoRepository housingRepository,
    IUnitOfWork unitOfWork,
    ILocalStorageService storageService)
    : IRequestHandler<UpdateRentHouseListingCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(UpdateRentHouseListingCommand request, CancellationToken ct)
    {
        var entity = await housingRepository.GetHouseInfoByIdAsync(request.Id, ct);
        if (entity == null)
            return StandardResponse.Failure(new ApiError("housing.notfound", "Listing not found."));

        if (entity.UserId != request.UserId)
            return StandardResponse.Failure(new ApiError("auth.forbidden", "You do not own this listing."));
        
        // 4. Update core properties
        entity.HouseType = request.HouseType;
        entity.PropertyType = request.PropertyType;
        entity.Description = request.Description;
        entity.Bedrooms = request.Bedrooms;
        entity.Bathrooms = request.Bathrooms;
        entity.Size = request.Sqft;
        entity.StartingPrice = request.MonthlyRent;
        entity.SecurityDeposit = request.SecurityDeposit;
        entity.BrokerFee = request.BrokerFee;
        entity.MonthlyCostRange = request.MonthlyCostRange;
        entity.BuildingType = request.BuildingType;
        entity.FloorLevel = request.FloorLevel;
        entity.YearBuilt = request.BuiltIn;
        entity.RenovatedIn = request.RenovatedIn;
        entity.MoveInDate = request.MoveInDate;
        entity.MoveOutDate = request.MoveOutDate;
        entity.MaxOccupants = request.MaxOccupants;
        entity.UnitNumber = request.UnitNumber;
        entity.GoogleMapLink = request.GoogleMap;
        entity.HeatingSystem = request.Heating;
        entity.CoolingSystem = request.Cooling;
        entity.TemperatureControl = request.TemperatureControl;
        entity.LaundryTypes = request.Laundry;
        entity.Amenities = request.Amenities;
        entity.NearbyTransportation = request.NearbyTransportation;
        entity.RentHousingPrograms = request.AcceptedHousingPrograms;
        entity.IsShortTermStayAllowed = request.ShortTermStayAllowed;
        entity.IsShortStayEligible = request.ShortStayEligiblity;
        entity.IsAcceptsHousingVouchers = request.AcceptsHousingVouchers;
        entity.IsFamilyAndKidsFriendly = request.FamilyAndKidsFriendly;
        entity.IsFurnished = request.Furnished;
        entity.IsPetsFriendly = request.PetsFriendly;
        entity.IsSmokingAllowed = request.SmokingAllowed;
        entity.IsAccessibilityFriendly = request.AccessibilityFriendly;
        entity.RentKitchenType = request.SharedKitchenType;
        entity.RentBathroomType = request.SharedBathroomType;
        entity.RentingAboutCurrentResident = request.AboutCurrentResident;
        entity.RentingRulesAndPolicies = request.UnitRulesAndPolicies;
        entity.RentingRoommateGroupChat = request.RoommatesGroupChat;
        entity.RentingLeaseType = request.LeaseType;
        entity.Borough = request.Borough;
        entity.Neighborhood = request.Neighborhood;
        entity.ZipCode = request.ZipCode;
        entity.FullAddress = request.FullAddress;
        entity.AddDirectApplyLink = request.AddDirectApplyLink;
        entity.AllowColisterEditing = request.AllowColisterEditing;
        entity.IsPublished = request.IsPublished;
        entity.CoListingIds = request.CoListing;

        // Handle Attachments - Delete first to avoid Id 0 collision with newly added items
        if (request.DeletedPhotoIds != null && request.DeletedPhotoIds.Any())
        {
            var attachmentsToRemove = entity.Attachments
                .Where(a => a.Id > 0 && request.DeletedPhotoIds.Contains(a.Id))
                .ToList();

            foreach (var attachment in attachmentsToRemove)
            {
                var fileName = attachment.Url.Replace("@local://", "");
                storageService.DeleteFile(fileName, "housing");
                entity.Attachments.Remove(attachment);
            }
        }

        if (request.NewPhotos != null && request.NewPhotos.Any())
        {
            foreach (var file in request.NewPhotos)
            {
                var fileName = await storageService.SaveFileAsync(file, "housing", ct);
                entity.Attachments.Add(new HousingAttachment { Url = "@local://" + fileName });
            }
        }

        await unitOfWork.SaveChangesAsync(ct);

        return StandardResponse.Success();
    }
}