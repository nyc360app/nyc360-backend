using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Entities.Topics;
using NYC360.Domain.Enums;
using NYC360.Infrastructure.Persistence.Seeders.Base;

namespace NYC360.Infrastructure.Persistence.Seeders;

public class TopicSeeder : ISeeder
{
    public async Task SeedAsync(IServiceProvider services)
    {
        var unitOfWork = services.GetRequiredService<IUnitOfWork>();
        var topicRepository = services.GetRequiredService<ITopicRepository>();

        // Check if topics already exist
        var existingTopics = await topicRepository.GetAllAsync(default);
        if (existingTopics.Any()) return;

        var topics = new List<Topic>
        {
            // Community
            new() { Name = "Community Announcement", Category = Category.Community },
            new() { Name = "Local Initiative", Category = Category.Community },
            
            // Culture
            new() { Name = "Cultural Post", Category = Category.Culture },
            new() { Name = "Exhibition / Program", Category = Category.Culture },
            new() { Name = "Artist or Cultural Project", Category = Category.Culture },
            new() { Name = "Cultural Experience Recommendation", Category = Category.Culture },
            
            // Education
            new() { Name = "Education Program", Category = Category.Education },
            new() { Name = "Learning Resource", Category = Category.Education },
            new() { Name = "Workshop / Training", Category = Category.Education },
            
            // Health
            new() { Name = "Public Health Program", Category = Category.Health },
            new() { Name = "Clinical Wellness Program", Category = Category.Health },
            new() { Name = "Health Resource", Category = Category.Health },
            new() { Name = "Care Access Information", Category = Category.Health },
            
            // Housing
            new() { Name = "Housing Program / Initiative", Category = Category.Housing },
            new() { Name = "Housing Assistance Resource", Category = Category.Housing },
            new() { Name = "Housing Policy / Market Update", Category = Category.Housing },
            new() { Name = "Tenant / Owner Guidance", Category = Category.Housing },
            
            // Lifestyle
            new() { Name = "Lifestyle Post", Category = Category.Lifestyle },
            new() { Name = "Lifestyle Wellness Activity", Category = Category.Lifestyle },
            new() { Name = "Lifestyle Experience Recommendation", Category = Category.Lifestyle },
            new() { Name = "City Living Guide", Category = Category.Lifestyle },
            
            // Legal
            new() { Name = "Legal Resource", Category = Category.Legal },
            new() { Name = "Rights & Compliance Guidance ", Category = Category.Legal },
            new() { Name = "Legal Aid Services ", Category = Category.Legal },
            
            // News
            new() { Name = "City Update", Category = Category.News },
            new() { Name = "Editorial Series", Category = Category.News },
            
            // Professions
            new() { Name = "Professional Service", Category = Category.Professions },
            new() { Name = "Career Resource", Category = Category.Professions },
            new() { Name = "Hiring Insights", Category = Category.Professions },
            
            // Social 
            new() { Name = "Social Initiative", Category = Category.Social },
            new() { Name = "Volunteer Opportunity", Category = Category.Social },
            new() { Name = "Impact Update", Category = Category.Social },
            
            // Transportation
            new() { Name = "Transit Update", Category = Category.Transportation },
            new() { Name = "Accessibility Notice", Category = Category.Transportation },
        };

        foreach (var topic in topics)
        {
            await topicRepository.AddAsync(topic, default);
        }

        await unitOfWork.SaveChangesAsync(default);
    }
}
