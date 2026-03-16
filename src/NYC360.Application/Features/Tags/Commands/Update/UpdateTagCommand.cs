using NYC360.Domain.Enums.Tags;
using NYC360.Domain.Wrappers;
using NYC360.Domain.Enums;
using MediatR;

namespace NYC360.Application.Features.Tags.Commands.Update;

public record UpdateTagCommand(
    int Id,
    string Name,
    TagType Type,
    Category? Division,
    int? ParentTagId
) : IRequest<StandardResponse>;