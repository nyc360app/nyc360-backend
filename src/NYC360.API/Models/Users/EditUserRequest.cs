namespace NYC360.API.Models.Users;

public record EditUserRequest(
    int Id,
    string FirstName,
    string LastName,
    string Email,
    string Bio,
    IFormFile? Avatar
);