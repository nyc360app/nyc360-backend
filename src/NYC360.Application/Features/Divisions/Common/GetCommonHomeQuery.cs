using NYC360.Domain.Dtos.Pages;
using NYC360.Domain.Wrappers;
using NYC360.Domain.Enums;
using MediatR;

namespace NYC360.Application.Features.Divisions.Common;

public record GetCommonHomeQuery(
    Category? Division, 
    int? UserId,
    int Limit = 5
) : IRequest<StandardResponse<DivisionHomeDto>>;