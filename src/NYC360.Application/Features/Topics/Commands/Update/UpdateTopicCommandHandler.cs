using MediatR;
using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Topics.Commands.Update;

public class UpdateTopicCommandHandler(
    ITopicRepository topicRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateTopicCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(UpdateTopicCommand request, CancellationToken cancellationToken)
    {
        var topic = await topicRepository.GetByIdAsync(request.Id, cancellationToken);
        if (topic == null)
        {
            return StandardResponse.Failure(new ApiError("topic.notfound", "Topic not found."));
        }

        // Check uniqueness if name changed
        if (topic.Name != request.Name || topic.Category != request.Category)
        {
            var isUnique = await topicRepository.IsTopicNameUnique(request.Name, request.Category);
            if (!isUnique)
            {
                return StandardResponse.Failure(new ApiError("topic.duplicate", "A topic with this name already exists in this category."));
            }
        }

        topic.Name = request.Name;
        topic.Category = request.Category;

        topicRepository.Update(topic);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return StandardResponse.Success();
    }
}
