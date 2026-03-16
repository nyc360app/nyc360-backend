using MediatR;
using NYC360.Domain.Dtos.User;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Users.Queries.List;

public record GetUsersListQuery(int Page, int PageSize, string? Search = null) 
    : PagedRequest(Page, PageSize), IRequest<PagedResponse<UserDashboardDetailDto>>;