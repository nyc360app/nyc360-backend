using MediatR;
using NYC360.Domain.Enums;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Topics.Commands.Create;

public class CreateTopicCommand : IRequest<StandardResponse<int>>
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Category? Category { get; set; }
}
