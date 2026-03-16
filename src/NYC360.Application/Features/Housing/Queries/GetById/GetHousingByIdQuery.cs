using NYC360.Domain.Dtos.Housing;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Housing.Queries.GetById;

public record GetHousingByIdQuery(int? UserId, int Id) : IRequest<StandardResponse<HousingDetailsDto>>;