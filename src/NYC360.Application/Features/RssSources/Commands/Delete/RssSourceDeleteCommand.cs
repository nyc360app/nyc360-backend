using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.RssSources.Commands.Delete;

public record RssSourceDeleteCommand(int Id)
    : IRequest<StandardResponse>;