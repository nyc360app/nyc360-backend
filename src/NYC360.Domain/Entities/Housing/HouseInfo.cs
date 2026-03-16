using NYC360.Domain.Entities.User;
using NYC360.Domain.Enums.Housing;

namespace NYC360.Domain.Entities.Housing;

public class HouseInfo
{
    public int Id { get; set; }
    public bool IsRenting { get; set; }
    public string Description { get; set; }
    public HousingType HouseType { get; set; }
    public PropertyType PropertyType { get; set; }
    public DateTime MoveInDate { get; set; }
    public DateTime? MoveOutDate { get; set; }
    public int MaxOccupants { get; set; }
    public string? GoogleMapLink { get; set; }
    public int Bedrooms { get; set; }
    public int Bathrooms { get; set; }
    public int? Size { get; set; }
    public int StartingPrice { get; set; }
    public int? SecurityDeposit { get; set; }
    public int? BrokerFee { get; set; }
    public int? MonthlyCostRange { get; set; }
    public BuildingType BuildingType { get; set; }
    public int? YearBuilt { get; set; }
    public int? RenovatedIn { get; set; }
    public string? FloorLevel { get; set; }
    public int? LegalUnitCount { get; set; }
    public HeatingSystem HeatingSystem { get; set; }
    public CoolingSystem CoolingSystem { get; set; }
    public TemperatureControl TemperatureControl { get; set; }
    public List<LaundryType> LaundryTypes { get; set; } = null!;
    public List<RentHousingProgram>? RentHousingPrograms { get; set; }
    public List<BuyerHousingProgram>? BuyerHousingProgram { get; set; }
    public List<NearbyTransportation>? NearbyTransportation { get; set; }
    public List<HousingAmenities>? Amenities { get; set; }
    public bool? IsShortTermStayAllowed { get; set; }
    public bool? IsShortStayEligible { get; set; }
    public bool IsFurnished { get; set; }
    public bool IsAcceptsHousingVouchers { get; set; }
    public bool IsFamilyAndKidsFriendly { get; set; }
    public bool IsPetsFriendly { get; set; }
    public bool IsAccessibilityFriendly { get; set; }
    public bool IsSmokingAllowed { get; set; }
    public LeaseType? RentingLeaseType { get; set; }
    public RentPrivacyType? RentPrivacyType { get; set; }
    public RentBathroomType? RentBathroomType { get; set; }
    public RentKitchenType? RentKitchenType { get; set; }
    public string? RentingAboutCurrentResident { get; set; }
    public string? RentingRulesAndPolicies { get; set; }
    public string? RentingRoommateGroupChat { get; set; }
    public bool AddDirectApplyLink { get; set; }
    public bool? AllowColisterEditing { get; set; }
    public List<int>? CoListingIds { get; set; }
    public bool IsPublished { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Address
    public string Borough { get; set; }
    public string Neighborhood { get; set; } = string.Empty;
    public string ZipCode { get; set; }
    public string? FullAddress { get; set; }
    public string? UnitNumber { get; set; }
    
    
    public UserProfile? User { get; set; }
    public HouseListingAuthorization? HouseListingAuthorization { get; set; }
    public ICollection<HousingAttachment> Attachments { get; set; } = new List<HousingAttachment>();
}