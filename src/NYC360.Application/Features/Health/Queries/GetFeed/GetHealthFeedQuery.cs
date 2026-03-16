using NYC360.Domain.Dtos.Pages;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Health.Queries.GetFeed;

public record GetHealthFeedQuery(int? UserId, int? LocationId) : IRequest<StandardResponse<HealthFeedDto>>;