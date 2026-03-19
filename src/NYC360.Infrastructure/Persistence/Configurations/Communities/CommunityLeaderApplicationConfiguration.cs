using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NYC360.Domain.Entities.Communities;
using NYC360.Domain.Enums.Communities;

namespace NYC360.Infrastructure.Persistence.Configurations.Communities;

public class CommunityLeaderApplicationConfiguration : IEntityTypeConfiguration<CommunityLeaderApplication>
{
    public void Configure(EntityTypeBuilder<CommunityLeaderApplication> builder)
    {
        builder.ToTable("CommunityLeaderApplications");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.FullName).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Email).IsRequired().HasMaxLength(255);
        builder.Property(x => x.PhoneNumber).IsRequired().HasMaxLength(50);
        builder.Property(x => x.CommunityName).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Location).IsRequired().HasMaxLength(200);
        builder.Property(x => x.ProfileLink).HasMaxLength(500);
        builder.Property(x => x.Motivation).IsRequired().HasMaxLength(2000);
        builder.Property(x => x.Experience).IsRequired().HasMaxLength(2000);
        builder.Property(x => x.WeeklyAvailability).IsRequired().HasMaxLength(500);
        builder.Property(x => x.VerificationFileUrl).IsRequired().HasMaxLength(260);
        builder.Property(x => x.AdminNotes).HasMaxLength(2000);
        builder.Property(x => x.Status).IsRequired().HasDefaultValue(CommunityLeaderApplicationStatus.Pending);
        builder.Property(x => x.CreatedAt).IsRequired();

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.CreatedAt);
        builder.HasIndex(x => x.UserId)
            .HasDatabaseName("IX_CommunityLeaderApplications_UserId_Pending")
            .HasFilter($"[{nameof(CommunityLeaderApplication.Status)}] = {(int)CommunityLeaderApplicationStatus.Pending}")
            .IsUnique();
    }
}
