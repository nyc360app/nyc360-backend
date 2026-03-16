using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using NYC360.Domain.Entities.Events;

namespace NYC360.Infrastructure.Persistence.Configurations.Events;

public class EventTicketTierConfiguration : IEntityTypeConfiguration<EventTicketTier>
{
    public void Configure(EntityTypeBuilder<EventTicketTier> builder)
    {
        // Set decimal precision (addresses the recurring WARN)
        builder
            .Property(t => t.Price)
            .HasColumnType("decimal(18,2)"); 
            
        // Relationship to Event - CRITICAL FIX
        // We set this to RESTRICT to break the cycle. 
        // When an Event is deleted, EventTickets will be deleted via the direct EventTickets relationship.
        builder
            .HasOne(t => t.Event)
            .WithMany(e => e.Tiers)
            .HasForeignKey(t => t.EventId)
            .OnDelete(DeleteBehavior.Restrict); // <-- RESTRICT TO BREAK THE CYCLE

        // Relationship to Tickets (Keep CASCADE: deleting a Tier should delete its Tickets)
        builder
            .HasMany(t => t.Tickets)
            .WithOne(et => et.Tier)
            .HasForeignKey(et => et.TierId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}