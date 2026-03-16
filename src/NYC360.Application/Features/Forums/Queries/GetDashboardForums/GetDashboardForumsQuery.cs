using MediatR;
using NYC360.Domain.Dtos.Forums;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Forums.Queries.GetDashboardForums;

public record GetDashboardForumsQuery() : IRequest<StandardResponse<List<ForumDto>>>;
