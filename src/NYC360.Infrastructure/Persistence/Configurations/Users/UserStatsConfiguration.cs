using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NYC360.Domain.Entities.User;

namespace NYC360.Infrastructure.Persistence.Configurations.Users;

public class UserStatsConfiguration : IEntityTypeConfiguration<UserStats>
{
    public void Configure(EntityTypeBuilder<UserStats> builder)
    {
        // 1. Set the Primary Key
        builder.HasKey(s => s.UserId);

        // 2. Configure 1:1 Relationship with UserProfile
        // This stops EF from creating 'UserId1' shadow properties
        builder.HasOne(s => s.User)
            .WithOne(p => p.Stats)
            .HasForeignKey<UserStats>(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);
            
    }
}