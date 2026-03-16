using MediatR;
using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Entities.Topics;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Topics.Commands.Create;

public class CreateTopicCommandHandler(
    ITopicRepository topicRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateTopicCommand, StandardResponse<int>>
{
    public async Task<StandardResponse<int>> Handle(CreateTopicCommand request, CancellationToken cancellationToken)
    {
        var isUnique = await topicRepository.IsTopicNameUnique(request.Name, request.Category);
        if (!isUnique)
        {
            return StandardResponse<int>.Failure(new ApiError("topic.duplicate", "A topic with this name already exists in this category."));
        }

        var topic = new Topic
        {
            Name = request.Name,
            Category = request.Category
        };

        await topicRepository.AddAsync(topic, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return StandardResponse<int>.Success(topic.Id);
    }
}
