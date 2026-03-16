using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NYC360.Domain.Entities.Events;

namespace NYC360.Infrastructure.Persistence.Configurations.Events;

public class EventAttendanceConfiguration : IEntityTypeConfiguration<EventAttendance>
{
    public void Configure(EntityTypeBuilder<EventAttendance> builder)
    {
        // 1. Define the Composite Key
        builder.HasKey(ea => new { ea.EventId, ea.UserId }); 

        // 2. Relationship to Event (Event -> EventAttendance)
        // Note: The previous code had this relationship defined twice. We remove the redundancy.
        builder
            .HasOne(ea => ea.Event)
            .WithMany(e => e.AttendanceRecords)
            .HasForeignKey(ea => ea.EventId)
            .OnDelete(DeleteBehavior.Cascade); 

        // 3. Relationship to ApplicationUser (User -> EventAttendance) - CRITICAL FIX
        // We use the navigation property (.HasOne(ea => ea.User)) to fix the shadow property issue.
        // We set OnDelete(DeleteBehavior.Restrict) to break the cascade cycle.
        builder
            .HasOne(ea => ea.User) // <-- Use the actual navigation property name
            .WithMany() // Assuming ApplicationUser does not have a navigation collection back to EventAttendance
            .HasForeignKey(ea => ea.UserId)
            .OnDelete(DeleteBehavior.Restrict); // <-- FIXES THE MULTIPLE CASCADE PATHS
        
        // Ensure you also apply the decimal configuration for EventTicketTier in its configuration file 
        // and the Point configuration for Event in its file.
        // Example: 
        // builder.Property(ea => ea.Status).IsRequired();
    }
}