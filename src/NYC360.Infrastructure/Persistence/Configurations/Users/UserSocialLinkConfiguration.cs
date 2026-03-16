using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NYC360.Domain.Entities.User;

namespace NYC360.Infrastructure.Persistence.Configurations.Users;

public class UserSocialLinkConfiguration : IEntityTypeConfiguration<UserSocialLink>
{
    public void Configure(EntityTypeBuilder<UserSocialLink> builder)
    {
        builder.HasKey(sl => sl.Id);

        // Explicitly map UserId to prevent 'UserId1' shadow property
        builder.HasOne(sl => sl.User)
            .WithMany(p => p.SocialLinks)
            .HasForeignKey(sl => sl.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}