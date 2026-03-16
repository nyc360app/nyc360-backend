using MediatR;
using NYC360.Domain.Enums;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.RssSources.Commands.ConnectRequest;

public record RssFeedConnectionRequestCreateCommand(
    string Url, 
    Category Category, 
    string Name, 
    string? Description, 
    string? ImageUrl,
    int RequesterId) : IRequest<StandardResponse>;
