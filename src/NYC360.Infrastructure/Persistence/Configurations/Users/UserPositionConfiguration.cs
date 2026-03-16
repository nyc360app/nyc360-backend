using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NYC360.Domain.Entities.User;

namespace NYC360.Infrastructure.Persistence.Configurations.Users;

public class UserPositionConfiguration : IEntityTypeConfiguration<UserPosition>
{
    public void Configure(EntityTypeBuilder<UserPosition> builder)
    {
        builder.HasKey(p => p.Id);

        // Break the cascade path here
        builder.HasOne(p => p.UserProfile)
            .WithMany(u => u.Positions)
            .HasForeignKey(p => p.UserProfileId)
            .OnDelete(DeleteBehavior.NoAction); // This solves the SQL Error 1785
    }
}