using NYC360.Domain.Dtos.Housing;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Housing.Queries.Home;

public record GetHousingHomeQuery(int? UserId) : IRequest<StandardResponse<HousingHomeDto>>;