using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using NYC360.Domain.Entities.Locations;
using NYC360.Domain.Entities.User;

namespace NYC360.Infrastructure.Persistence.Configurations.Users;

public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.HasKey(p => p.UserId);

        builder.Property(p => p.FirstName)
            .HasMaxLength(100);

        builder.Property(p => p.LastName)
            .HasMaxLength(100);

        builder.Property(p => p.Bio)
            .HasMaxLength(500);
        
        builder.HasOne(p => p.User)
            .WithOne(u => u.Profile)
            .HasForeignKey<UserProfile>(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Profile -> Stats (Cascade)
        builder.HasOne(p => p.Stats)
            .WithOne(s => s.User)
            .HasForeignKey<UserStats>(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Profile -> Interests (Cascade)
        builder.HasMany(p => p.Interests)
            .WithOne(s => s.User)
            .HasForeignKey(i => i.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Profile -> SocialLinks (Cascade)
        builder.HasMany(p => p.SocialLinks)
            .WithOne(s => s.User)
            .HasForeignKey(sl => sl.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(p => p.JobApplications)
            .WithOne(ja => ja.Applicant)
            .HasForeignKey(ja => ja.ApplicantId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(p => p.Positions)
            .WithOne(u => u.UserProfile)
            .HasForeignKey(p => p.UserProfileId)
            .OnDelete(DeleteBehavior.NoAction);
        
        // Profile -> Tags (Cascade)
        builder.HasMany(p => p.Tags)
            .WithOne(t => t.User)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Address
        builder.HasOne(p => p.Address)
            .WithMany()
            .HasForeignKey(p => p.AddressId)
            .OnDelete(DeleteBehavior.SetNull);
        
        builder.HasOne(p => p.Address)
            .WithOne(a => a.ManagedByUser)
            .HasForeignKey<Address>(a => a.ManagedByUserId)
            .OnDelete(DeleteBehavior.NoAction);
        
        // user specific type info
        builder.HasOne(p => p.BusinessInfo)
            .WithOne(b => b.UserProfile)
            .HasForeignKey<BusinessInfo>(b => b.UserId);

        builder.HasOne(p => p.VisitorInfo)
            .WithOne(b => b.UserProfile)
            .HasForeignKey<VisitorInfo>(b => b.UserId);
        
        builder.HasOne(p => p.OrganizationInfo)
            .WithOne(b => b.UserProfile)
            .HasForeignKey<OrganizationInfo>(b => b.UserId);
        
        builder.HasOne(p => p.NewYorkerInfo)
            .WithOne(b => b.UserProfile)
            .HasForeignKey<NewYorkerInfo>(b => b.UserId);
    }
}