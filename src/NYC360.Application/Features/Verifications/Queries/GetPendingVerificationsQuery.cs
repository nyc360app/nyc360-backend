using NYC360.Domain.Dtos.Tags;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Verifications.Queries;

public record GetPendingVerificationsQuery(int Page, int PageSize) 
    : PagedRequest(Page, PageSize), IRequest<PagedResponse<PendingTagVerificationDto>>;