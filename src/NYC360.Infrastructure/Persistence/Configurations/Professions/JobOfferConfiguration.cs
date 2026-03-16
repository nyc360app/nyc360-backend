using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NYC360.Domain.Entities.Professions;
using Microsoft.EntityFrameworkCore;

namespace NYC360.Infrastructure.Persistence.Configurations.Professions;

public class JobOfferConfiguration : IEntityTypeConfiguration<JobOffer>
{
    public void Configure(EntityTypeBuilder<JobOffer> builder)
    {
        builder.ToTable("JobOffers");

        builder.HasKey(j => j.Id);

        builder.Property(j => j.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(j => j.Description)
            .IsRequired();

        builder.Property(j => j.SalaryMin)
            .HasPrecision(18, 2);

        builder.Property(j => j.SalaryMax)
            .HasPrecision(18, 2);

        builder.HasIndex(j => j.IsActive);
        builder.HasIndex(j => j.CreatedAt);

        builder.HasOne(j => j.Author)
            .WithMany() 
            .HasForeignKey(j => j.AuthorId)
            .OnDelete(DeleteBehavior.ClientCascade); // Bypasses SQL Cycle Error

        builder.HasOne(j => j.Address)
            .WithMany() 
            .HasForeignKey(j => j.AddressId)
            .OnDelete(DeleteBehavior.Cascade);

        // This ensures that if a JobOffer is deleted, its apps are gone
        builder.HasMany(j => j.Applications)
            .WithOne(a => a.JobOffer)
            .HasForeignKey(a => a.JobOfferId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}