using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using NYC360.Domain.Entities.Events;
using NYC360.Domain.Entities.User;

namespace NYC360.Infrastructure.Persistence.Configurations.Events;

public class EventModerationActionConfiguration : IEntityTypeConfiguration<EventModerationAction>
{
    public void Configure(EntityTypeBuilder<EventModerationAction> builder)
    {
        builder.HasKey(ma => ma.Id);

        builder
            .HasOne<Event>()
            .WithMany(e => e.ModerationActions)
            .HasForeignKey(ma => ma.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(ma => ma.AdminId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(ma => new { ma.EventId, ma.CreatedAt });
    }
}