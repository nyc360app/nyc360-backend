using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Forums.Commands.DeleteForum;

public record DeleteForumCommand(int Id) : IRequest<StandardResponse>;
