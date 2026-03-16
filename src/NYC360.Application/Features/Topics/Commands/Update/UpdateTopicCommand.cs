using MediatR;
using NYC360.Domain.Enums;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Topics.Commands.Update;

public class UpdateTopicCommand : IRequest<StandardResponse>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Category? Category { get; set; }
}
