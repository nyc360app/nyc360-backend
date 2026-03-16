using NYC360.Domain.Dtos.Common;
using NYC360.Domain.Wrappers;
using NYC360.Domain.Enums;
using MediatR;

namespace NYC360.Application.Features.Common.Queries.GlobalSearch;

public record GlobalSearchQuery(
    int? UserId,
    string Term,
    Category? Division,
    int Limit
) : IRequest<StandardResponse<GlobalSearchDto>>;