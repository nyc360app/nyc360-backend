using MediatR;
using NYC360.Domain.Enums;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.RssSources.Commands.ConnectRequest;

public record RssFeedConnectionRequestUpdateCommand(
    int RequestId, 
    RssConnectionStatus Status,
    string? AdminNote,
    Category? Category,
    int ProcessedByUserId) : IRequest<StandardResponse>;
