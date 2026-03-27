using NYC360.Domain.Dtos.News;
using NYC360.Domain.Enums.News;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Contracts.Services;

public interface INewsPollService
{
    Task<StandardResponse<NewsPollCreateResultDto>> CreateAsync(int userId, NewsPollCreateInput input, CancellationToken ct);
    Task<PagedResponse<NewsPollListItemDto>> GetMineAsync(int userId, NewsPollStatus? status, int page, int pageSize, CancellationToken ct);
    Task<PagedResponse<NewsPollSummaryDto>> GetPublishedAsync(int? requesterUserId, int page, int pageSize, CancellationToken ct);
    Task<StandardResponse<NewsPollDetailsDto>> GetByIdAsync(int? requesterUserId, int pollId, CancellationToken ct);
    Task<StandardResponse<NewsPollCreateResultDto>> UpdateAsync(int userId, int pollId, NewsPollUpdateInput input, CancellationToken ct);
    Task<StandardResponse<NewsPollVoteResultDto>> VoteAsync(int userId, int pollId, IReadOnlyCollection<int> optionIds, CancellationToken ct);
    Task<StandardResponse<NewsPollResultsDto>> GetResultsAsync(int? requesterUserId, int pollId, CancellationToken ct);
    Task<PagedResponse<NewsPollAdminPendingDto>> GetPendingAsync(int reviewerUserId, int page, int pageSize, string? search, CancellationToken ct);
    Task<StandardResponse<NewsPollReviewResultDto>> ReviewAsync(int reviewerUserId, int pollId, bool approved, string? adminComment, CancellationToken ct);
}
