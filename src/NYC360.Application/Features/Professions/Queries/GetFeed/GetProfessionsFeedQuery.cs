using NYC360.Domain.Dtos.Professions;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Professions.Queries.GetFeed;

public record GetProfessionsFeedQuery(int? UserId) : IRequest<StandardResponse<ProfessionsFeedDto>>;