using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using NYC360.Domain.Entities.Events;

namespace NYC360.Infrastructure.Persistence.Configurations.Events;

public class EventOrganizerConfiguration : IEntityTypeConfiguration<EventStaff>
{
    public void Configure(EntityTypeBuilder<EventStaff> builder)
    {
        builder.HasKey(eo => new { eo.EventId, eo.UserId });

        builder
            .HasOne(eo => eo.Event)
            .WithMany(e => e.Staff)
            .HasForeignKey(eo => eo.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(eo => eo.User)
            .WithMany()
            .HasForeignKey(eo => eo.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}