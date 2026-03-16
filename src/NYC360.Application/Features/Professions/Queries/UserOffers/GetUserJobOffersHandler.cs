using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.Professions;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Professions.Queries.UserOffers;

public class GetUserJobOffersHandler(IProfessionsRepository repo) 
    : IRequestHandler<GetUserJobOffersQuery, PagedResponse<JobOfferManageDto>>
{
    public async Task<PagedResponse<JobOfferManageDto>> Handle(GetUserJobOffersQuery request, CancellationToken ct)
    {
        var (items, total) = await repo.GetUserJobOffersAsync(
            request.UserId, 
            request.IsActive, 
            request.Page, 
            request.PageSize, 
            ct);

        var dtos = items.Select(j => new JobOfferManageDto(
            j.Id,
            j.Title,
            j.SalaryMin,
            j.SalaryMax,
            j.IsActive,
            j.CreatedAt,
            0 // TODO: change when can have Total applications
        )).ToList();

        return PagedResponse<JobOfferManageDto>.Create(
            dtos, 
            request.Page, 
            request.PageSize,
            total
        );
    }
}