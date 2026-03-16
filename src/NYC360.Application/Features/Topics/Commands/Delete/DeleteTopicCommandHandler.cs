using MediatR;
using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Topics.Commands.Delete;

public class DeleteTopicCommandHandler(
    ITopicRepository topicRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteTopicCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(DeleteTopicCommand request, CancellationToken cancellationToken)
    {
        var topic = await topicRepository.GetByIdAsync(request.Id, cancellationToken);
        if (topic == null)
        {
            return StandardResponse.Failure(new ApiError("topic.notfound", "Topic not found."));
        }

        // The database constraint (Restrict) will prevent deletion if posts are linked.
        // We could also check explicitly if we want a better error message.
        
        topicRepository.Remove(topic);
        
        try 
        {
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
            return StandardResponse.Failure(new ApiError("topic.in_use", "Cannot delete topic because it is being used by posts."));
        }

        return StandardResponse.Success();
    }
}
