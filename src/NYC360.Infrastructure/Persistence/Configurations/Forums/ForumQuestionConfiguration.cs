using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NYC360.Domain.Entities.Forums;

namespace NYC360.Infrastructure.Persistence.Configurations.Forums;

public class ForumQuestionConfiguration : IEntityTypeConfiguration<ForumQuestion>
{
    public void Configure(EntityTypeBuilder<ForumQuestion> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(300);
            
        builder.Property(x => x.Content)
            .IsRequired();
            
        builder.Property(x => x.Slug)
            .IsRequired()
            .HasMaxLength(350);
            
        builder.HasIndex(x => x.Slug)
            .IsUnique();

        builder.HasOne(x => x.Author)
            .WithMany()
            .HasForeignKey(x => x.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Answers)
            .WithOne(x => x.Question)
            .HasForeignKey(x => x.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
