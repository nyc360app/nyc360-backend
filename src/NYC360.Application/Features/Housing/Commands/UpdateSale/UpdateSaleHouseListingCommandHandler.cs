using NYC360.Application.Contracts.Persistence;
using NYC360.Application.Contracts.Storage;
using NYC360.Domain.Entities.Housing;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Housing.Commands.UpdateSale;

public class UpdateSaleHouseListingCommandHandler(
    IHouseInfoRepository housingRepository,
    IUnitOfWork unitOfWork,
    ILocalStorageService storageService)
    : IRequestHandler<UpdateSaleHouseListingCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(UpdateSaleHouseListingCommand request, CancellationToken ct)
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
        entity.LegalUnitCount = request.LegalUnitCount;
        entity.Size = request.Sqft;
        entity.StartingPrice = request.AskingPrice;
        entity.SecurityDeposit = request.DownPayment;
        entity.BrokerFee = request.BrokerFee;
        entity.MonthlyCostRange = request.MonthlyCostRange;
        entity.BuildingType = request.BuildingType;
        entity.FloorLevel = request.FloorLevel;
        entity.YearBuilt = request.BuiltIn;
        entity.RenovatedIn = request.RenovatedIn;
        entity.MoveInDate = request.OpeningDate;
        entity.MaxOccupants = request.SuggestedOccupants;
        entity.UnitNumber = request.UnitNumber;
        entity.GoogleMapLink = request.GoogleMap;
        entity.HeatingSystem = request.Heating;
        entity.CoolingSystem = request.Cooling;
        entity.TemperatureControl = request.TemperatureControl;
        entity.LaundryTypes = request.Laundry;
        entity.Amenities = request.Amenities;
        entity.NearbyTransportation = request.NearbyTransportation;
        entity.BuyerHousingProgram = request.AcceptedBuyerPrograms;
        entity.IsAcceptsHousingVouchers = request.AcceptsHousingVouchers;
        entity.IsFamilyAndKidsFriendly = request.FamilyAndKidsFriendly;
        entity.IsFurnished = request.Furnished;
        entity.IsPetsFriendly = request.PetsFriendly;
        entity.IsSmokingAllowed = request.SmokingAllowed;
        entity.IsAccessibilityFriendly = request.AccessibilityFriendly;
        entity.Borough = request.Borough;
        entity.Neighborhood = request.Neighborhood;
        entity.ZipCode = request.ZipCode;
        entity.FullAddress = request.FullAddress;
        entity.AddDirectApplyLink = request.AddDirectApplyLink;
        entity.AllowColisterEditing = request.AllowColisterEditing;
        entity.CoListingIds = request.CoListing;
        entity.IsPublished = request.IsPublished;

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