using NYC360.Domain.Entities.Locations;
using NYC360.Domain.Enums.Professions;
using NYC360.Domain.Entities.User;

namespace NYC360.Domain.Entities.Professions;

public class JobOffer
{
    public int Id { get; set; }
    
    // Ownership
    public int AuthorId { get; set; }
    public UserProfile? Author { get; set; }
    
    // Job Details
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Requirements { get; set; } 
    public string? Benefits { get; set; }
    public string? Responsablities { get; set; }
    public string? CompanyName { get; set; } // Optional override if different from Profile
    
    // Logistics
    public decimal? SalaryMin { get; set; }
    public decimal? SalaryMax { get; set; }
    public WorkArrangement WorkArrangement { get; set; }
    public EmploymentType EmploymentType { get; set; }
    public EmploymentLevel EmploymentLevel { get; set; }
    
    // Location (Crucial for Neighborhood Pulse)
    public int? AddressId { get; set; }
    public Address? Address { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ExpiresAt { get; set; }
    public bool IsActive { get; set; } = true;
    
    public ICollection<JobApplication> Applications { get; set; } = new List<JobApplication>();
}