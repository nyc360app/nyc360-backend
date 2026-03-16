namespace NYC360.API.Models.Users.ProfileUpdate;

public record UpdateBasicInfoRequest(
    string FirstName, 
    string LastName, 
    string Headline, 
    string Bio, 
    int? LocationId
);