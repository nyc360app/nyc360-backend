using System.ComponentModel.DataAnnotations;
using NYC360.Domain.Entities.Communities;
using NYC360.Domain.Entities.Professions;
using NYC360.Domain.Entities.Locations;
using NYC360.Domain.Entities.Posts;
using NYC360.Domain.Entities.Tags;

namespace NYC360.Domain.Entities.User;

public class UserProfile
{
    [Key]
    public int UserId { get; set; }
    
    public string FirstName { get; set; } = string.Empty;
    public string? LastName { get; set; } = string.Empty;
    public string? Headline { get; set; } = string.Empty;
    public string? Bio { get; set; } = string.Empty;
    public string? Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; } = string.Empty;
    
    public string? AvatarUrl { get; set; } = string.Empty;
    public string? CoverImageUrl { get; set; } = string.Empty;
   
    public ApplicationUser? User { get; set; }
    public int? AddressId { get; set; }
    
    public Address? Address { get; set; }
    public UserStats? Stats { get; set; } = new();
    public VisitorInfo? VisitorInfo { get; set; }
    public BusinessInfo? BusinessInfo { get; set; }
    public OrganizationInfo? OrganizationInfo { get; set; }
    public NewYorkerInfo? NewYorkerInfo { get; set; }
    public ICollection<UserInterest>? Interests { get; set; } = new List<UserInterest>();
    public ICollection<UserSavedPost>? SavedPosts { get; set; } = new List<UserSavedPost>();
    public ICollection<UserTag>? Tags { get; init; } = new List<UserTag>(); 
    public ICollection<UserSocialLink> SocialLinks { get; set; } = new List<UserSocialLink>();
    public ICollection<UserPosition> Positions { get; set; } = new List<UserPosition>();
    public ICollection<UserEducation> Educations { get; set; } = new List<UserEducation>();
    public ICollection<CommunityMember> CommunityMemberships { get; set; } = new List<CommunityMember>();
    public ICollection<JobApplication> JobApplications { get; set; } = new List<JobApplication>();
    public ICollection<JobOffer> JobOffers { get; set; } = new List<JobOffer>();
    public ICollection<Post> Posts { get; set; } = new List<Post>();
    public ICollection<CommunityDisbandRequest> CommunityDisbandRequests { get; set; } = new List<CommunityDisbandRequest>();

    public string GetFullName() => $"{FirstName} {LastName}";
}