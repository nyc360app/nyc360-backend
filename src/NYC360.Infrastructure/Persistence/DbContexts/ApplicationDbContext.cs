using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using NYC360.Domain.Entities.Professions;
using NYC360.Domain.Entities.Communities;
using NYC360.Domain.Entities.Locations;
using NYC360.Domain.Entities.Support;
using NYC360.Domain.Entities.Housing;
using Microsoft.EntityFrameworkCore;
using NYC360.Domain.Entities.Events;
using NYC360.Domain.Entities.Posts;
using NYC360.Domain.Entities.User;
using NYC360.Domain.Entities.Tags;
using NYC360.Domain.Entities.Forums;
using NYC360.Domain.Entities;
using NYC360.Domain.Entities.Topics;
using NYC360.Domain.Entities.SpaceListings;

namespace NYC360.Infrastructure.Persistence.DbContexts;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser, ApplicationRole, int>(options)
{
    // users
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<UserSocialLink> UserSocialLinks { get; set; }
    public DbSet<UserStats> UserStats { get; set; }
    public DbSet<UserInterest> UserInterests { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<UserPosition> UserPositions { get; set; }
    public DbSet<UserEducation> UserEducations { get; set; }
    public DbSet<UserSavedPost> UserSavedPosts { get; set; }
    public DbSet<VisitorInfo> VisitorInfos { get; set; }
    public DbSet<BusinessInfo> BusinessInfos { get; set; }
    public DbSet<OrganizationInfo> OrganizationInfos { get; set; }
    public DbSet<OrganizationService> OrganizationServices { get; set; }
    
    // Rss Feeds
    public DbSet<RssFeedSource> RssFeedSources { get; set; }
    public DbSet<RssFeedConnectionRequest> RssFeedConnectionRequests { get; set; }
    
    // Location
    public DbSet<Location> Locations { get; set; }
    public DbSet<Address> Addresses { get; set; }
    
    // posts
    public DbSet<Post> Posts { get; set; }
    public DbSet<Topic> Topics { get; set; }
    public DbSet<PostStats> PostStats { get; set; }
    public DbSet<PostInteraction> PostUserInteractions { get; set; }
    public DbSet<PostViewEvent> PostViewEvents { get; set; }
    public DbSet<PostAttachment> PostAttachments { get; set; }
    public DbSet<PostFlag> PostFlags { get; set; }
    public DbSet<PostLink> PostLinks { get; set; }
    public DbSet<PostComment> PostComments { get; set; }
    public DbSet<PostCommentInteraction> PostCommentLikes { get; set; }
    public DbSet<PostCommentStats> PostCommentStats { get; set; }
    
    // tags
    public DbSet<Tag> Tags { get; set; }
    public DbSet<UserTag> UserTags { get; set; }
    public DbSet<PostTag> PostTags { get; set; }
    public DbSet<TagVerificationRequest> TagVerificationRequests { get; set; }

    // events
    public DbSet<Event> Events { get; set; }
    public DbSet<EventAttachment> EventAttachments { get; set; }
    public DbSet<EventAttendance> EventAttendance { get; set; }
    public DbSet<EventTicketTier> EventTicketTiers { get; set; }
    public DbSet<EventTicket> EventTickets { get; set; }
    public DbSet<EventStaff> EventStaffs { get; set; }
    public DbSet<EventModerationAction> EventModerationActions { get; set; }
    
    // communities
    public DbSet<Community> Communities { get; set; }
    public DbSet<CommunityMember> CommunityMembers { get; set; }
    public DbSet<CommunityJoinRequest> CommunityJoinRequests { get; set; }
    public DbSet<CommunityDisbandRequest> CommunityDisbandRequests { get; set; }
    public DbSet<CommunityLeaderApplication> CommunityLeaderApplications { get; set; }
    
    // professions
    public DbSet<JobOffer> JobOffers { get; set; }
    public DbSet<JobApplication> JobApplications { get; set; }
    
    // Tickets
    public DbSet<SupportTicket> SupportTickets { get; set; }
    
    // Housing
    public DbSet<HouseInfo> HouseInfos { get; set; }
    public DbSet<HousingAttachment> HousingAttachments { get; set; }
    public DbSet<HousingRequest> HousingRequests { get; set; }
    public DbSet<HouseListingAuthorization> HouseListingAuthorizations { get; set; }
    public DbSet<HouseListingAuthorizationAvailability> HouseListingAuthorizationAvailabilities { get; set; }
    public DbSet<HouseListingAuthorizationAttachment> HouseListingAuthorizationAttachments { get; set; }

    // Space Listings
    public DbSet<SpaceListing> SpaceListings { get; set; }
    public DbSet<SpaceListingAttachment> SpaceListingAttachments { get; set; }
    public DbSet<SpaceListingSocialLink> SpaceListingSocialLinks { get; set; }
    public DbSet<SpaceListingHour> SpaceListingHours { get; set; }
    public DbSet<SpaceListingReviewEntry> SpaceListingReviewEntries { get; set; }
    
    // forums
    public DbSet<Forum> Forums { get; set; }
    public DbSet<ForumQuestion> ForumQuestions { get; set; }
    public DbSet<ForumAnswer> ForumAnswers { get; set; }
    public DbSet<ForumModerator> ForumModerators { get; set; }
    
    
    // common
    public DbSet<VerificationDocument> VerificationDocuments { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
