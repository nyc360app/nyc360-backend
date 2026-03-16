using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using NYC360.Domain.Entities.Events;

namespace NYC360.Infrastructure.Persistence.Configurations.Events;

public class EventTicketConfiguration : IEntityTypeConfiguration<EventTicket>
{
    public void Configure(EntityTypeBuilder<EventTicket> builder)
    {
        // 1. Relationship to Event (This should be CASCADE, deleting Event deletes Tickets)
        builder
            .HasOne(et => et.Event)
            .WithMany() // Assuming Event has no navigation collection back to EventTicket
            .HasForeignKey(et => et.EventId)
            .OnDelete(DeleteBehavior.Cascade); 
        
        // 2. Relationship to TicketTier (CASCADE is correct, deleting Tier deletes dependent Tickets)
        builder
            .HasOne(et => et.Tier)
            .WithMany(t => t.Tickets)
            .HasForeignKey(et => et.TierId)
            .OnDelete(DeleteBehavior.Cascade); 

        // 3. Relationship to ApplicationUser (Purchaser) - FIXES THE NEW CYCLE
        builder
            .HasOne(et => et.User) // Assuming the navigation property is named 'User'
            .WithMany() 
            .HasForeignKey(et => et.UserId)
            .OnDelete(DeleteBehavior.Restrict); // <-- THE CRITICAL FIX
    }
}