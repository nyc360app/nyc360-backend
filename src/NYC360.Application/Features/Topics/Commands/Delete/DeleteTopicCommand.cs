using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Topics.Commands.Delete;

public class DeleteTopicCommand : IRequest<StandardResponse>
{
    public int Id { get; set; }
}
