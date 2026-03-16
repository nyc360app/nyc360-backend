using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Posts.Commands.DashboardDelete;

public sealed record PostDeleteAdminCommand(int PostId) : IRequest<StandardResponse>;