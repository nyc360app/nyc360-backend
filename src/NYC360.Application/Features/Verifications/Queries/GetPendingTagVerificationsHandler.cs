using MediatR;
using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.Common;
using NYC360.Domain.Dtos.Tags;
using NYC360.Domain.Dtos.User;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Verifications.Queries;

public class GetPendingTagVerificationsHandler(IVerificationRepository repository)
    : IRequestHandler<GetPendingVerificationsQuery, PagedResponse<PendingTagVerificationDto>>
{
    public async Task<PagedResponse<PendingTagVerificationDto>> Handle(GetPendingVerificationsQuery request, CancellationToken ct)
    {
        var (items, totalCount) = await repository.GetPagedPendingRequestsAsync(request.Page, request.PageSize, ct);

        var dtos = items.Select(r => new PendingTagVerificationDto(
            r.Id,
            r.Reason,
            r.CreatedAt,
            new TagMinimalDto(r.TargetTagId, r.TargetTag!.Name),
            UserMinimalInfoDto.Map(r.User!),
            r.Documents.Select(d => new VerificationDocDto(d.Id, d.DocumentType, d.FileUrl)).ToList()
        ));

        return PagedResponse<PendingTagVerificationDto>.Create(dtos, request.Page, request.PageSize, totalCount);
    }
}