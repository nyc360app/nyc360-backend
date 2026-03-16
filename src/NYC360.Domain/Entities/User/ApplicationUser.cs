using Microsoft.AspNetCore.Identity;
using NYC360.Domain.Enums;
using NYC360.Domain.Enums.Users;

namespace NYC360.Domain.Entities.User;

public class ApplicationUser : IdentityUser<int>
{
    public UserType Type { get; set; } = UserType.Normal;
    public bool IsPending { get; set; }
    public UserProfile? Profile { get; set; }
    
    public string GetFullName() => Profile != null ? $"{Profile!.FirstName} {Profile!.LastName}" : UserName!;
}