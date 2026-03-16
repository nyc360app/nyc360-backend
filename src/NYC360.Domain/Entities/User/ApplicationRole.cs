using Microsoft.AspNetCore.Identity;

namespace NYC360.Domain.Entities.User;

public class ApplicationRole : IdentityRole<int>
{
    public int ContentLimit { get; set; } = 500;
    
    public ApplicationRole() : base() {}
    public ApplicationRole(string roleName) : base(roleName) {}
}