using NYC360.Domain.Dtos.Location;
using NYC360.Domain.Enums;

namespace NYC360.API.Models.Authentication.Register;

public sealed record NewYorkerRegisterRequest(
    string FirstName, 
    string? LastName, 
    string Username,
    string Email, 
    string Password,
    AddressInputDto Address,
    List<Category> Interests,
    bool IsInterestedInVolunteering,
    bool IsOpenToAttendingLocalEvents,
    bool FollowNeighborhoodUpdates,
    bool MakeProfilePublic,
    bool DisplayNeighborhood,
    bool AllowMessagesFromVerifiedOrganizations
);