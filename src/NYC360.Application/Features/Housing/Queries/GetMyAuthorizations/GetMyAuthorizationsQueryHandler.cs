using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.Housing;
using NYC360.Domain.Wrappers;
using MediatR;
using NYC360.Domain.Dtos.Posts;

namespace NYC360.Application.Features.Housing.Queries.GetMyAuthorizations;

public class GetMyAuthorizationsQueryHandler(IHouseListingAuthorizationRepository repository)
    : IRequestHandler<GetMyAuthorizationsQuery, StandardResponse<List<HouseListingAuthorizationDto>>>
{
    public async Task<StandardResponse<List<HouseListingAuthorizationDto>>> Handle(GetMyAuthorizationsQuery request, CancellationToken cancellationToken)
    {
        var authorizations = await repository.GetByUserIdAsync(request.UserId, cancellationToken);
        
        var dtos = authorizations.Select(HouseListingAuthorizationDto.Map).ToList();

        return StandardResponse<List<HouseListingAuthorizationDto>>.Success(dtos);
    }
}
