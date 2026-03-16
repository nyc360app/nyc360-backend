using NYC360.Domain.Entities.Housing;
using NYC360.Domain.Enums.Housing;
using NYC360.Domain.Dtos.Posts;
using NYC360.Domain.Dtos.User;

namespace NYC360.Domain.Dtos.Housing;

public record HousingDto(
    int Id,
    bool IsRenting,
    HousingType HouseType,
    DateTime MoveInOrOpeningDate,
    DateTime? MoveOutDate,
    PropertyType PropertyType,
    int? LegalUnitCount,
    
    string Borough,
    string ZipCode,
    int MaxOrSuggestedOccupants,
    string Neighborhood,
    string? FullAddress,
    string? UnitNumber,
    string? GoogleMap,
    List<NearbyTransportation>? NearbyTransportation,
    int Bedrooms,
    int Bathrooms,
    
    int StartingOrAskingPrice,
    int? SecurityDepositOrDownPayment,
    int? BrokerFee,
    int? MonthlyCostRange,
    
    BuildingType BuildingType,
    int? BuiltIn,
    int? RenovatedIn,
    int? Sqft,
    string? FloorLevel,
    HeatingSystem Heating,
    CoolingSystem Cooling,
    TemperatureControl TemperatureControl,
    
    List<LaundryType> LaundryTypes,
    List<HousingAmenities>? Amenities,
    
    bool? ShortTermStayAllowed,
    bool? ShortStayEligible,
    bool Furnished,
    bool AcceptsHousingVouchers,
    bool FamilyAndKidsFriendly,
    bool PetsFriendly,
    bool AccessibilityFriendly,
    bool SmokingAllowed,
    
    List<RentHousingProgram>? RentHousingPrograms,
    List<BuyerHousingProgram>? BuyerHousingProgram,
    string Description,
    
    LeaseType? RentingLeaseType,
    RentPrivacyType? RentPrivacyType,
    RentBathroomType? RentBathroomType,
    RentKitchenType? RentKitchenType,
    string? RentingAboutCurrentResident,
    string? RentingRulesAndPolicies,
    string? RentingRoommateGroupChat,
        
    bool AddDirectApplyLink,
    List<int>? CoListers,
    UserMinimalInfoDto Author,
    List<AttachmentDto> Attachments
);

public static class HousingDtoExtensions
{
    extension(HousingDto)
    {
        public static HousingDto Map(HouseInfo info) => new(
            info.Id,
            info.IsRenting,
            info.HouseType,
            info.MoveInDate,
            info.MoveOutDate,
            info.PropertyType,
            info.LegalUnitCount,
            info.Borough,
            info.ZipCode,
            info.MaxOccupants,
            info.Neighborhood,
            info.FullAddress,
            info.UnitNumber,
            info.GoogleMapLink,
            info.NearbyTransportation,
            info.Bedrooms,
            info.Bathrooms,
            info.StartingPrice,
            info.SecurityDeposit,
            info.BrokerFee,
            info.MonthlyCostRange,
            info.BuildingType,
            info.YearBuilt,
            info.RenovatedIn,
            info.Size,
            info.FloorLevel,
            info.HeatingSystem,
            info.CoolingSystem,
            info.TemperatureControl,
            info.LaundryTypes,
            info.Amenities,
            info.IsShortTermStayAllowed,
            info.IsShortStayEligible,
            info.IsFurnished,
            info.IsAcceptsHousingVouchers,
            info.IsFamilyAndKidsFriendly,
            info.IsPetsFriendly,
            info.IsAccessibilityFriendly,
            info.IsSmokingAllowed,
            info.RentHousingPrograms,
            info.BuyerHousingProgram,
            info.Description,
            info.RentingLeaseType,
            info.RentPrivacyType,
            info.RentBathroomType,
            info.RentKitchenType,
            info.RentingAboutCurrentResident,
            info.RentingRulesAndPolicies,
            info.RentingRoommateGroupChat,
            info.AddDirectApplyLink,
            info.CoListingIds,
            UserMinimalInfoDto.Map(info.User!),
            info.Attachments.Select(AttachmentDto.Map).ToList()
        );
    }
}