using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using NYC360.Domain.Entities.User;

namespace NYC360.Infrastructure.Persistence.Configurations.Users;

public class UserInterestConfiguration : IEntityTypeConfiguration<UserInterest>
{
    public void Configure(EntityTypeBuilder<UserInterest> builder)
    {
        builder.ToTable("UserInterests");

        builder.HasKey(ui => new { ui.UserId, ui.Category });
        
        builder.HasOne(ui => ui.User)
            .WithMany(u => u.Interests) // The navigation property added to ApplicationUser
            .HasForeignKey(ui => ui.UserId)
            .OnDelete(DeleteBehavior.Cascade); // If the user is deleted, their interests are also deleted
        
        builder.Property(ui => ui.Category)
            .IsRequired();
    }
}