using System.ComponentModel.DataAnnotations;
using NYC360.Domain.Enums.Users;

namespace NYC360.Domain.Entities.User;

public class OrganizationService
{
    [Key]
    public int OrganizationId { get; set; }
    public OrganizationInfo? Organization { get; set; }
    
    public OrganizationServices Service { get; set; }
}