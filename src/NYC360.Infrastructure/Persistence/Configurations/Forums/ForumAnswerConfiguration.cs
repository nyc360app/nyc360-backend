using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NYC360.Domain.Entities.Forums;

namespace NYC360.Infrastructure.Persistence.Configurations.Forums;

public class ForumAnswerConfiguration : IEntityTypeConfiguration<ForumAnswer>
{
    public void Configure(EntityTypeBuilder<ForumAnswer> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Content)
            .IsRequired();

        builder.HasOne(x => x.Author)
            .WithMany()
            .HasForeignKey(x => x.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
