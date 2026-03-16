using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using NYC360.Domain.Enums.Users;

namespace NYC360.Domain.Entities.User;

public class OrganizationInfo
{
    [Key, ForeignKey(nameof(UserProfile))]
    public int UserId { get; set; }
    
    public OrganizationType OrganizationType { get; set; }
    public ServiceArea ServiceArea { get; set; }
    public OrganizationFundType FundType { get; set; }
    public bool IsTaxExempt { get; set; } // 501(c)(3) status [cite: 69]
    public bool IsNysRegistered { get; set; } // Registered in New York State [cite: 70]
    
    public UserProfile? UserProfile { get; set; }
    public ICollection<OrganizationService> Services { get; set; } = new List<OrganizationService>();
}