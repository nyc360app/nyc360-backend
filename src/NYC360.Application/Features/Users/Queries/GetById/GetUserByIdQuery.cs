using MediatR;
using NYC360.Domain.Dtos.User;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Users.Queries.GetById;

public record GetUserByIdQuery(int Id) : IRequest<StandardResponse<UserDashboardDetailDto>>;