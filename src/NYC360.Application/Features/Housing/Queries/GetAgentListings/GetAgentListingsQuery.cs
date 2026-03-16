using NYC360.Domain.Dtos.Housing;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Housing.Queries.GetAgentListings;

public record GetAgentListingsQuery(int UserId, int PageNumber, int PageSize) 
    : IRequest<PagedResponse<AgentListingDto>>;
