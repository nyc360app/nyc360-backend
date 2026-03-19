using Microsoft.Extensions.DependencyInjection;
using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Constants;
using NYC360.Domain.Entities.Tags;
using NYC360.Domain.Enums;
using NYC360.Domain.Enums.Tags;
using NYC360.Infrastructure.Persistence.Seeders.Base;

namespace NYC360.Infrastructure.Persistence.Seeders;

public class NewsTagSeeder : ISeeder
{
    public async Task SeedAsync(IServiceProvider services)
    {
        var unitOfWork = services.GetRequiredService<IUnitOfWork>();
        var tagRepository = services.GetRequiredService<ITagRepository>();

        var changed = false;

        foreach (var name in new[]
                 {
                     NewsDepartmentTags.PublisherBadgeName,
                     NewsDepartmentTags.AssistantPublisherBadgeName,
                     NewsDepartmentTags.JournalistBadgeName,
                     NewsDepartmentTags.AuthorBadgeName,
                     NewsDepartmentTags.DocumentorBadgeName,
                     NewsDepartmentTags.ContributorBadgeName,
                     NewsDepartmentTags.TraineeJournalistBadgeName,
                     NewsDepartmentTags.ListNewsOrganizationInSpaceName,
                     NewsDepartmentTags.VerifiedPublisherName,
                     NewsDepartmentTags.ProbationaryPublisherName
                 })
        {
            var tag = await tagRepository.GetByNameAsync(name, default);
            if (tag == null)
            {
                await tagRepository.AddAsync(new Tag
                {
                    Name = name,
                    Type = TagType.Professional,
                    Division = Category.News
                });
                changed = true;
                continue;
            }

            if (tag.Type != TagType.Professional || tag.Division != Category.News)
            {
                tag.Type = TagType.Professional;
                tag.Division = Category.News;
                tagRepository.Update(tag);
                changed = true;
            }
        }

        if (changed)
            await unitOfWork.SaveChangesAsync(default);
    }
}
