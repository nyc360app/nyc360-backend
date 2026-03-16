using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using NYC360.Domain.Enums.Users;

namespace NYC360.Domain.Entities.User;

public class BusinessInfo
{
    [Key, ForeignKey(nameof(UserProfile))]
    public int UserId { get; set; }
    
    public Industry Industry { get; set; }
    public BusinessSize BusinessSize { get; set; }
    public ServiceArea ServiceArea { get; set; }
    public Services Services { get; set; }
    public OwnershipType OwnershipType { get; set; }
    public bool IsPublic { get; set; }
    public bool IsLicensedInNyc { get; set; }
    public bool IsInsured { get; set; }
    
    public UserProfile? UserProfile { get; set; }
}