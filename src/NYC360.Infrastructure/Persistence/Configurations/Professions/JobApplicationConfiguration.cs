using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NYC360.Domain.Entities.Professions;
using Microsoft.EntityFrameworkCore;

namespace NYC360.Infrastructure.Persistence.Configurations.Professions;

public class JobApplicationConfiguration : IEntityTypeConfiguration<JobApplication>
{
    public void Configure(EntityTypeBuilder<JobApplication> builder)
    {
        builder.ToTable("JobApplications");

        builder.HasKey(a => a.Id);

        // Prevent duplicate applications: One user per JobOffer
        builder.HasIndex(a => new { a.JobOfferId, a.ApplicantId }).IsUnique();

        builder.Property(a => a.CoverLetter)
            .HasMaxLength(3000);

        // Relationships
        builder.HasOne(a => a.Applicant)
            .WithMany() 
            .HasForeignKey(a => a.ApplicantId)
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.HasOne(a => a.JobOffer)
            .WithMany(j => j.Applications)
            .HasForeignKey(a => a.JobOfferId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}