using System.Data;
using System.Data.Common;
using System.Text.Json;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NYC360.Application.Contracts.Persistence;
using NYC360.Application.Contracts.Services;
using NYC360.Application.Contracts.Storage;
using NYC360.Domain.Dtos.News;
using NYC360.Domain.Enums.News;
using NYC360.Domain.Wrappers;
using NYC360.Infrastructure.Persistence.DbContexts;

namespace NYC360.Infrastructure.Services;

public class NewsPollService(
    ApplicationDbContext context,
    INewsAuthorizationService newsAuthorizationService,
    ILocationRepository locationRepository,
    ILocalStorageService localStorageService,
    ISlugService slugService)
    : INewsPollService
{
    private const int MinOptions = 2;
    private const int MaxOptions = 6;

    public async Task<StandardResponse<NewsPollCreateResultDto>> CreateAsync(int userId, NewsPollCreateInput input, CancellationToken ct)
    {
        var access = await newsAuthorizationService.GetAccessAsync(userId, ct);
        if (access == null || !access.CanSubmitContent)
        {
            return StandardResponse<NewsPollCreateResultDto>.Failure(
                new ApiError("news.polls.forbidden", "You do not have permission to create News polls."));
        }

        var title = input.Title?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(title))
        {
            return StandardResponse<NewsPollCreateResultDto>.Failure(
                new ApiError("news.polls.title.required", "Poll title is required."));
        }

        var question = input.Question?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(question))
        {
            return StandardResponse<NewsPollCreateResultDto>.Failure(
                new ApiError("news.polls.question.required", "Poll question is required."));
        }

        if (input.ClosesAt.HasValue && input.ClosesAt.Value <= DateTime.UtcNow)
        {
            return StandardResponse<NewsPollCreateResultDto>.Failure(
                new ApiError("news.polls.invalid_close_date", "Poll close date must be in the future."));
        }

        if (input.LocationId.HasValue)
        {
            var locationExists = await locationRepository.ExistsAsync(input.LocationId.Value, ct);
            if (!locationExists)
            {
                return StandardResponse<NewsPollCreateResultDto>.Failure(
                    new ApiError("location.notFound", "Location not found."));
            }
        }

        var options = NormalizeOptions(input.Options);
        if (options.Count < MinOptions || options.Count > MaxOptions)
        {
            return StandardResponse<NewsPollCreateResultDto>.Failure(
                new ApiError("news.polls.invalid_options", "Poll options must contain between 2 and 6 unique values."));
        }

        var tags = NormalizeTags(input.Tags);
        var description = string.IsNullOrWhiteSpace(input.Description) ? null : input.Description.Trim();
        var slug = await slugService.GenerateUniqueSlugAsync(
            title,
            s => SlugExistsAsync(s, ct),
            ct);

        var status = access.CanPublishContent ? NewsPollStatus.Published : NewsPollStatus.PendingReview;
        var isFeatured = access.IsStaff && input.IsFeatured;
        var coverImageUrl = input.CoverImage != null
            ? "@local://" + await localStorageService.SaveFileAsync(input.CoverImage, "news-polls", ct)
            : null;

        await using var tx = await context.Database.BeginTransactionAsync(ct);
        var connection = await GetOpenConnectionAsync(ct);

        var pollId = await InsertPollAsync(
            connection,
            tx,
            userId,
            title,
            question,
            description,
            slug,
            coverImageUrl,
            input.ClosesAt,
            input.AllowMultipleAnswers,
            input.ShowResultsBeforeVoting,
            isFeatured,
            input.LocationId,
            JsonSerializer.Serialize(tags),
            status,
            ct);

        await ReplaceOptionsAsync(connection, tx, pollId, options, ct);
        await tx.CommitAsync(ct);

        return StandardResponse<NewsPollCreateResultDto>.Success(
            new NewsPollCreateResultDto(pollId, ToStatusLabel(status, input.ClosesAt), slug));
    }

    public async Task<PagedResponse<NewsPollListItemDto>> GetMineAsync(
        int userId,
        NewsPollStatus? status,
        int page,
        int pageSize,
        CancellationToken ct)
    {
        page = page <= 0 ? 1 : page;
        pageSize = pageSize <= 0 ? 20 : Math.Min(pageSize, 100);

        var connection = await GetOpenConnectionAsync(ct);

        var totalCount = await GetMineTotalCountAsync(connection, userId, status, ct);
        var items = await GetMineItemsAsync(connection, userId, status, page, pageSize, ct);

        return PagedResponse<NewsPollListItemDto>.Create(items, page, pageSize, totalCount);
    }

    public async Task<PagedResponse<NewsPollSummaryDto>> GetPublishedAsync(int? requesterUserId, int page, int pageSize, CancellationToken ct)
    {
        page = page <= 0 ? 1 : page;
        pageSize = pageSize <= 0 ? 20 : Math.Min(pageSize, 100);

        var connection = await GetOpenConnectionAsync(ct);
        var totalCount = await GetPublishedTotalCountAsync(connection, ct);
        var items = await GetPublishedItemsAsync(connection, requesterUserId, page, pageSize, ct);

        return PagedResponse<NewsPollSummaryDto>.Create(items, page, pageSize, totalCount);
    }

    public async Task<StandardResponse<NewsPollDetailsDto>> GetByIdAsync(int? requesterUserId, int pollId, CancellationToken ct)
    {
        var poll = await GetPollAsync(pollId, ct);
        if (poll == null)
        {
            return StandardResponse<NewsPollDetailsDto>.Failure(
                new ApiError("news.polls.not_found", "Poll not found."));
        }

        var access = await GetAccessOrNullAsync(requesterUserId, ct);
        var isStaff = access?.IsStaff ?? false;
        var canModerate = access?.CanModerateContent ?? false;
        var isOwner = requesterUserId.HasValue && requesterUserId.Value == poll.CreatorUserId;
        var isPublished = poll.Status == NewsPollStatus.Published;

        if (!isPublished && !isOwner && !isStaff && !canModerate)
        {
            return StandardResponse<NewsPollDetailsDto>.Failure(
                new ApiError("news.polls.not_found", "Poll not found."));
        }

        var selectedOptionIds = requesterUserId.HasValue
            ? await GetUserSelectedOptionIdsAsync(pollId, requesterUserId.Value, ct)
            : [];
        var hasVoted = selectedOptionIds.Count > 0;

        var optionVotes = await GetOptionVoteCountsAsync(pollId, ct);
        var totalVotes = await GetTotalVotesAsync(pollId, ct);
        var canSeeResults = poll.ShowResultsBeforeVoting || hasVoted || isOwner || isStaff || canModerate;

        var effectiveStatus = ToStatusLabel(poll.Status, poll.ClosesAt);
        var canVote = requesterUserId.HasValue
            && poll.Status == NewsPollStatus.Published
            && !IsClosed(poll.ClosesAt)
            && !hasVoted;

        var canEdit = poll.Status == NewsPollStatus.PendingReview && (isOwner || isStaff || canModerate);

        var options = poll.Options
            .Select(o =>
            {
                var votes = canSeeResults && optionVotes.TryGetValue(o.Id, out var count) ? count : 0;
                var percent = canSeeResults && totalVotes > 0
                    ? Math.Round((double)votes * 100.0 / totalVotes, 2)
                    : 0;
                return new NewsPollOptionDto(o.Id, o.Text, votes, percent);
            })
            .ToList();

        return StandardResponse<NewsPollDetailsDto>.Success(
            new NewsPollDetailsDto(
                poll.Id,
                poll.Slug,
                poll.Title,
                poll.Question,
                poll.Description,
                NewsPollMediaPath.ToPublicPath(poll.CoverImageUrl),
                effectiveStatus,
                poll.AllowMultipleAnswers,
                poll.ShowResultsBeforeVoting,
                poll.IsFeatured,
                poll.CreatorUserId,
                poll.LocationId,
                DeserializeStringList(poll.TagsJson),
                poll.CreatedAt,
                poll.ClosesAt,
                hasVoted,
                selectedOptionIds,
                canEdit,
                canVote,
                canSeeResults ? totalVotes : 0,
                options));
    }

    public async Task<StandardResponse<NewsPollCreateResultDto>> UpdateAsync(
        int userId,
        int pollId,
        NewsPollUpdateInput input,
        CancellationToken ct)
    {
        var poll = await GetPollAsync(pollId, ct);
        if (poll == null)
        {
            return StandardResponse<NewsPollCreateResultDto>.Failure(
                new ApiError("news.polls.not_found", "Poll not found."));
        }

        var access = await newsAuthorizationService.GetAccessAsync(userId, ct);
        if (access == null)
        {
            return StandardResponse<NewsPollCreateResultDto>.Failure(
                new ApiError("news.polls.forbidden", "You do not have permission to update this poll."));
        }

        var isOwner = poll.CreatorUserId == userId;
        if (!isOwner && !access.IsStaff)
        {
            return StandardResponse<NewsPollCreateResultDto>.Failure(
                new ApiError("news.polls.forbidden", "You do not have permission to update this poll."));
        }

        if (poll.Status != NewsPollStatus.PendingReview)
        {
            return StandardResponse<NewsPollCreateResultDto>.Failure(
                new ApiError("news.polls.update_not_allowed", "Only pending-review polls can be updated."));
        }

        var title = input.Title?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(title))
        {
            return StandardResponse<NewsPollCreateResultDto>.Failure(
                new ApiError("news.polls.title.required", "Poll title is required."));
        }

        var question = input.Question?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(question))
        {
            return StandardResponse<NewsPollCreateResultDto>.Failure(
                new ApiError("news.polls.question.required", "Poll question is required."));
        }

        if (input.ClosesAt.HasValue && input.ClosesAt.Value <= DateTime.UtcNow)
        {
            return StandardResponse<NewsPollCreateResultDto>.Failure(
                new ApiError("news.polls.invalid_close_date", "Poll close date must be in the future."));
        }

        if (input.LocationId.HasValue)
        {
            var locationExists = await locationRepository.ExistsAsync(input.LocationId.Value, ct);
            if (!locationExists)
            {
                return StandardResponse<NewsPollCreateResultDto>.Failure(
                    new ApiError("location.notFound", "Location not found."));
            }
        }

        var options = NormalizeOptions(input.Options);
        if (options.Count < MinOptions || options.Count > MaxOptions)
        {
            return StandardResponse<NewsPollCreateResultDto>.Failure(
                new ApiError("news.polls.invalid_options", "Poll options must contain between 2 and 6 unique values."));
        }

        var description = string.IsNullOrWhiteSpace(input.Description) ? null : input.Description.Trim();
        var tagsJson = JsonSerializer.Serialize(NormalizeTags(input.Tags));
        var isFeatured = access.IsStaff && input.IsFeatured;
        var coverImageUrl = poll.CoverImageUrl;

        if (input.CoverImage != null)
        {
            coverImageUrl = "@local://" + await localStorageService.SaveFileAsync(input.CoverImage, "news-polls", ct);
        }

        await using var tx = await context.Database.BeginTransactionAsync(ct);
        var connection = await GetOpenConnectionAsync(ct);

        await UpdatePollAsync(
            connection,
            tx,
            pollId,
            title,
            question,
            description,
            coverImageUrl,
            input.ClosesAt,
            input.AllowMultipleAnswers,
            input.ShowResultsBeforeVoting,
            isFeatured,
            input.LocationId,
            tagsJson,
            ct);

        await ReplaceOptionsAsync(connection, tx, pollId, options, ct);
        await tx.CommitAsync(ct);

        return StandardResponse<NewsPollCreateResultDto>.Success(
            new NewsPollCreateResultDto(pollId, ToStatusLabel(poll.Status, input.ClosesAt), poll.Slug));
    }

    public async Task<StandardResponse<NewsPollVoteResultDto>> VoteAsync(
        int userId,
        int pollId,
        IReadOnlyCollection<int> optionIds,
        CancellationToken ct)
    {
        var poll = await GetPollAsync(pollId, ct);
        if (poll == null)
        {
            return StandardResponse<NewsPollVoteResultDto>.Failure(
                new ApiError("news.polls.not_found", "Poll not found."));
        }

        if (poll.Status != NewsPollStatus.Published || IsClosed(poll.ClosesAt))
        {
            return StandardResponse<NewsPollVoteResultDto>.Failure(
                new ApiError("news.polls.closed", "This poll is not accepting votes."));
        }

        var normalizedOptionIds = optionIds
            .Where(x => x > 0)
            .Distinct()
            .ToList();

        if (normalizedOptionIds.Count == 0)
        {
            return StandardResponse<NewsPollVoteResultDto>.Failure(
                new ApiError("news.polls.invalid_options", "At least one option must be selected."));
        }

        if (!poll.AllowMultipleAnswers && normalizedOptionIds.Count != 1)
        {
            return StandardResponse<NewsPollVoteResultDto>.Failure(
                new ApiError("news.polls.invalid_option_selection", "This poll allows only one selected option."));
        }

        if (poll.AllowMultipleAnswers && normalizedOptionIds.Count > MaxOptions)
        {
            return StandardResponse<NewsPollVoteResultDto>.Failure(
                new ApiError("news.polls.invalid_option_selection", "Too many options were selected."));
        }

        var validOptions = await CountMatchingOptionsAsync(pollId, normalizedOptionIds, ct);
        if (validOptions != normalizedOptionIds.Count)
        {
            return StandardResponse<NewsPollVoteResultDto>.Failure(
                new ApiError("news.polls.invalid_option_selection", "One or more selected options are invalid for this poll."));
        }

        try
        {
            await InsertVoteAsync(pollId, userId, normalizedOptionIds, ct);
        }
        catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
        {
            return StandardResponse<NewsPollVoteResultDto>.Failure(
                new ApiError("news.polls.already_voted", "You have already voted on this poll."));
        }

        var totalVotes = await GetTotalVotesAsync(pollId, ct);
        return StandardResponse<NewsPollVoteResultDto>.Success(
            new NewsPollVoteResultDto(pollId, totalVotes, normalizedOptionIds));
    }

    public async Task<StandardResponse<NewsPollResultsDto>> GetResultsAsync(int? requesterUserId, int pollId, CancellationToken ct)
    {
        var poll = await GetPollAsync(pollId, ct);
        if (poll == null)
        {
            return StandardResponse<NewsPollResultsDto>.Failure(
                new ApiError("news.polls.not_found", "Poll not found."));
        }

        var access = await GetAccessOrNullAsync(requesterUserId, ct);
        var isStaff = access?.IsStaff ?? false;
        var canModerate = access?.CanModerateContent ?? false;
        var isOwner = requesterUserId.HasValue && requesterUserId.Value == poll.CreatorUserId;

        if (poll.Status != NewsPollStatus.Published && !isOwner && !isStaff && !canModerate)
        {
            return StandardResponse<NewsPollResultsDto>.Failure(
                new ApiError("news.polls.not_found", "Poll not found."));
        }

        var hasVoted = requesterUserId.HasValue && (await GetUserSelectedOptionIdsAsync(pollId, requesterUserId.Value, ct)).Count > 0;
        if (!poll.ShowResultsBeforeVoting && !hasVoted && !isOwner && !isStaff && !canModerate)
        {
            return StandardResponse<NewsPollResultsDto>.Failure(
                new ApiError("news.polls.results_hidden", "Results are visible only after voting."));
        }

        var totalVotes = await GetTotalVotesAsync(pollId, ct);
        var optionVotes = await GetOptionVoteCountsAsync(pollId, ct);

        var options = poll.Options
            .Select(o =>
            {
                var votes = optionVotes.TryGetValue(o.Id, out var count) ? count : 0;
                var percent = totalVotes > 0 ? Math.Round((double)votes * 100.0 / totalVotes, 2) : 0;
                return new NewsPollOptionDto(o.Id, o.Text, votes, percent);
            })
            .ToList();

        return StandardResponse<NewsPollResultsDto>.Success(
            new NewsPollResultsDto(
                poll.Id,
                ToStatusLabel(poll.Status, poll.ClosesAt),
                totalVotes,
                poll.ShowResultsBeforeVoting,
                options));
    }

    public async Task<PagedResponse<NewsPollAdminPendingDto>> GetPendingAsync(
        int reviewerUserId,
        int page,
        int pageSize,
        string? search,
        CancellationToken ct)
    {
        var access = await newsAuthorizationService.GetAccessAsync(reviewerUserId, ct);
        if (access == null || !access.CanModerateContent)
        {
            return PagedResponse<NewsPollAdminPendingDto>.Failure(
                new ApiError("news.polls.forbidden", "You do not have permission to review News polls."));
        }

        page = page <= 0 ? 1 : page;
        pageSize = pageSize <= 0 ? 20 : Math.Min(pageSize, 100);
        var normalizedSearch = string.IsNullOrWhiteSpace(search) ? null : search.Trim();

        var connection = await GetOpenConnectionAsync(ct);
        var totalCount = await GetPendingTotalCountAsync(connection, normalizedSearch, ct);
        var items = await GetPendingItemsAsync(connection, normalizedSearch, page, pageSize, ct);

        return PagedResponse<NewsPollAdminPendingDto>.Create(items, page, pageSize, totalCount);
    }

    public async Task<StandardResponse<NewsPollReviewResultDto>> ReviewAsync(
        int reviewerUserId,
        int pollId,
        bool approved,
        string? adminComment,
        CancellationToken ct)
    {
        var access = await newsAuthorizationService.GetAccessAsync(reviewerUserId, ct);
        if (access == null || !access.CanModerateContent)
        {
            return StandardResponse<NewsPollReviewResultDto>.Failure(
                new ApiError("news.polls.forbidden", "You do not have permission to review News polls."));
        }

        var poll = await GetPollAsync(pollId, ct);
        if (poll == null)
        {
            return StandardResponse<NewsPollReviewResultDto>.Failure(
                new ApiError("news.polls.not_found", "Poll not found."));
        }

        if (poll.Status != NewsPollStatus.PendingReview)
        {
            return StandardResponse<NewsPollReviewResultDto>.Failure(
                new ApiError("news.polls.not_pending", "Only pending-review polls can be reviewed."));
        }

        var targetStatus = approved ? NewsPollStatus.Published : NewsPollStatus.Rejected;
        var connection = await GetOpenConnectionAsync(ct);

        await using var cmd = connection.CreateCommand();
        cmd.CommandText = """
            UPDATE [NewsPolls]
            SET [Status] = @status,
                [ReviewedByUserId] = @reviewedByUserId,
                [ReviewedAt] = @reviewedAt,
                [ModerationNote] = @moderationNote,
                [UpdatedAt] = @updatedAt
            WHERE [Id] = @pollId;
            """;
        AddParameter(cmd, "@status", (byte)targetStatus);
        AddParameter(cmd, "@reviewedByUserId", reviewerUserId);
        AddParameter(cmd, "@reviewedAt", DateTime.UtcNow);
        AddParameter(cmd, "@moderationNote", string.IsNullOrWhiteSpace(adminComment) ? null : adminComment.Trim(), DbType.String);
        AddParameter(cmd, "@updatedAt", DateTime.UtcNow);
        AddParameter(cmd, "@pollId", pollId);
        await cmd.ExecuteNonQueryAsync(ct);

        return StandardResponse<NewsPollReviewResultDto>.Success(
            new NewsPollReviewResultDto(pollId, ToStatusLabel(targetStatus, poll.ClosesAt), adminComment));
    }

    private async Task<NewsAccessDto?> GetAccessOrNullAsync(int? userId, CancellationToken ct)
    {
        if (!userId.HasValue)
            return null;

        return await newsAuthorizationService.GetAccessAsync(userId.Value, ct);
    }

    private async Task<bool> SlugExistsAsync(string slug, CancellationToken ct)
    {
        var connection = await GetOpenConnectionAsync(ct);
        await using var cmd = connection.CreateCommand();
        cmd.CommandText = "SELECT CASE WHEN EXISTS(SELECT 1 FROM [NewsPolls] WHERE [Slug] = @slug) THEN CAST(1 AS bit) ELSE CAST(0 AS bit) END;";
        AddParameter(cmd, "@slug", slug);
        var result = await cmd.ExecuteScalarAsync(ct);
        return result is bool b && b;
    }

    private static List<string> NormalizeOptions(List<string> options)
    {
        return options
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    private static List<string> NormalizeTags(List<string>? tags)
    {
        if (tags == null || tags.Count == 0)
            return [];

        return tags
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Take(20)
            .ToList();
    }

    private static bool IsClosed(DateTime? closesAt)
        => closesAt.HasValue && closesAt.Value <= DateTime.UtcNow;

    private static string ToStatusLabel(NewsPollStatus status, DateTime? closesAt)
    {
        if (status == NewsPollStatus.Published && IsClosed(closesAt))
            return "Closed";
        return status.ToString();
    }

    private static List<string> DeserializeStringList(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return [];

        try
        {
            return JsonSerializer.Deserialize<List<string>>(json) ?? [];
        }
        catch
        {
            return [];
        }
    }

    private static List<int> DeserializeIntList(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return [];

        try
        {
            return JsonSerializer.Deserialize<List<int>>(json) ?? [];
        }
        catch
        {
            return [];
        }
    }

    private async Task<DbConnection> GetOpenConnectionAsync(CancellationToken ct)
    {
        var connection = context.Database.GetDbConnection();
        if (connection.State != ConnectionState.Open)
            await connection.OpenAsync(ct);

        return connection;
    }

    private static void AddParameter(DbCommand command, string name, object? value, DbType? dbType = null)
    {
        var parameter = command.CreateParameter();
        parameter.ParameterName = name;
        if (dbType.HasValue)
            parameter.DbType = dbType.Value;
        parameter.Value = value ?? DBNull.Value;
        command.Parameters.Add(parameter);
    }

    private async Task<int> InsertPollAsync(
        DbConnection connection,
        IDbContextTransaction tx,
        int creatorUserId,
        string title,
        string question,
        string? description,
        string slug,
        string? coverImageUrl,
        DateTime? closesAt,
        bool allowMultipleAnswers,
        bool showResultsBeforeVoting,
        bool isFeatured,
        int? locationId,
        string tagsJson,
        NewsPollStatus status,
        CancellationToken ct)
    {
        await using var cmd = connection.CreateCommand();
        cmd.Transaction = tx.GetDbTransaction();
        cmd.CommandText = """
            INSERT INTO [NewsPolls]
            (
                [CreatorUserId], [Status], [Title], [Question], [Description], [Slug], [CoverImageUrl],
                [ClosesAt], [AllowMultipleAnswers], [ShowResultsBeforeVoting], [IsFeatured], [Category],
                [TagsJson], [LocationId], [CreatedAt], [UpdatedAt]
            )
            VALUES
            (
                @creatorUserId, @status, @title, @question, @description, @slug, @coverImageUrl,
                @closesAt, @allowMultipleAnswers, @showResultsBeforeVoting, @isFeatured, 7,
                @tagsJson, @locationId, @createdAt, @updatedAt
            );
            SELECT CAST(SCOPE_IDENTITY() AS INT);
            """;

        var now = DateTime.UtcNow;
        AddParameter(cmd, "@creatorUserId", creatorUserId);
        AddParameter(cmd, "@status", (byte)status);
        AddParameter(cmd, "@title", title);
        AddParameter(cmd, "@question", question);
        AddParameter(cmd, "@description", description, DbType.String);
        AddParameter(cmd, "@slug", slug);
        AddParameter(cmd, "@coverImageUrl", coverImageUrl, DbType.String);
        AddParameter(cmd, "@closesAt", closesAt, DbType.DateTime2);
        AddParameter(cmd, "@allowMultipleAnswers", allowMultipleAnswers);
        AddParameter(cmd, "@showResultsBeforeVoting", showResultsBeforeVoting);
        AddParameter(cmd, "@isFeatured", isFeatured);
        AddParameter(cmd, "@tagsJson", tagsJson);
        AddParameter(cmd, "@locationId", locationId, DbType.Int32);
        AddParameter(cmd, "@createdAt", now);
        AddParameter(cmd, "@updatedAt", now);

        var result = await cmd.ExecuteScalarAsync(ct);
        return result == null ? 0 : Convert.ToInt32(result);
    }

    private async Task UpdatePollAsync(
        DbConnection connection,
        IDbContextTransaction tx,
        int pollId,
        string title,
        string question,
        string? description,
        string? coverImageUrl,
        DateTime? closesAt,
        bool allowMultipleAnswers,
        bool showResultsBeforeVoting,
        bool isFeatured,
        int? locationId,
        string tagsJson,
        CancellationToken ct)
    {
        await using var cmd = connection.CreateCommand();
        cmd.Transaction = tx.GetDbTransaction();
        cmd.CommandText = """
            UPDATE [NewsPolls]
            SET [Title] = @title,
                [Question] = @question,
                [Description] = @description,
                [CoverImageUrl] = @coverImageUrl,
                [ClosesAt] = @closesAt,
                [AllowMultipleAnswers] = @allowMultipleAnswers,
                [ShowResultsBeforeVoting] = @showResultsBeforeVoting,
                [IsFeatured] = @isFeatured,
                [LocationId] = @locationId,
                [TagsJson] = @tagsJson,
                [UpdatedAt] = @updatedAt
            WHERE [Id] = @pollId;
            """;

        AddParameter(cmd, "@title", title);
        AddParameter(cmd, "@question", question);
        AddParameter(cmd, "@description", description, DbType.String);
        AddParameter(cmd, "@coverImageUrl", coverImageUrl, DbType.String);
        AddParameter(cmd, "@closesAt", closesAt, DbType.DateTime2);
        AddParameter(cmd, "@allowMultipleAnswers", allowMultipleAnswers);
        AddParameter(cmd, "@showResultsBeforeVoting", showResultsBeforeVoting);
        AddParameter(cmd, "@isFeatured", isFeatured);
        AddParameter(cmd, "@locationId", locationId, DbType.Int32);
        AddParameter(cmd, "@tagsJson", tagsJson);
        AddParameter(cmd, "@updatedAt", DateTime.UtcNow);
        AddParameter(cmd, "@pollId", pollId);
        await cmd.ExecuteNonQueryAsync(ct);
    }

    private async Task ReplaceOptionsAsync(
        DbConnection connection,
        IDbContextTransaction tx,
        int pollId,
        List<string> options,
        CancellationToken ct)
    {
        await using (var deleteCmd = connection.CreateCommand())
        {
            deleteCmd.Transaction = tx.GetDbTransaction();
            deleteCmd.CommandText = "DELETE FROM [NewsPollOptions] WHERE [PollId] = @pollId;";
            AddParameter(deleteCmd, "@pollId", pollId);
            await deleteCmd.ExecuteNonQueryAsync(ct);
        }

        for (var i = 0; i < options.Count; i++)
        {
            await using var insertCmd = connection.CreateCommand();
            insertCmd.Transaction = tx.GetDbTransaction();
            insertCmd.CommandText = """
                INSERT INTO [NewsPollOptions] ([PollId], [Text], [SortOrder])
                VALUES (@pollId, @text, @sortOrder);
                """;
            AddParameter(insertCmd, "@pollId", pollId);
            AddParameter(insertCmd, "@text", options[i]);
            AddParameter(insertCmd, "@sortOrder", i);
            await insertCmd.ExecuteNonQueryAsync(ct);
        }
    }

    private async Task<int> GetMineTotalCountAsync(DbConnection connection, int userId, NewsPollStatus? status, CancellationToken ct)
    {
        await using var cmd = connection.CreateCommand();
        cmd.CommandText = """
            SELECT COUNT(1)
            FROM [NewsPolls]
            WHERE [CreatorUserId] = @creatorUserId
              AND (@status IS NULL OR [Status] = @status);
            """;
        AddParameter(cmd, "@creatorUserId", userId);
        AddParameter(cmd, "@status", status.HasValue ? (byte)status.Value : null, DbType.Byte);
        var value = await cmd.ExecuteScalarAsync(ct);
        return value == null ? 0 : Convert.ToInt32(value);
    }

    private async Task<List<NewsPollListItemDto>> GetMineItemsAsync(
        DbConnection connection,
        int userId,
        NewsPollStatus? status,
        int page,
        int pageSize,
        CancellationToken ct)
    {
        await using var cmd = connection.CreateCommand();
        cmd.CommandText = """
            SELECT [Id], [Slug], [Title], [Question], [Status], [AllowMultipleAnswers], [ShowResultsBeforeVoting], [IsFeatured],
                   [CoverImageUrl], [CreatedAt], [ClosesAt],
                   (SELECT COUNT(1) FROM [NewsPollOptions] o WHERE o.[PollId] = p.[Id]) AS [OptionCount],
                   (SELECT COUNT(1) FROM [NewsPollVotes] v WHERE v.[PollId] = p.[Id]) AS [TotalVotes],
                   (SELECT CASE WHEN EXISTS(SELECT 1 FROM [NewsPollVotes] v2 WHERE v2.[PollId] = p.[Id] AND v2.[VoterUserId] = @voterUserId)
                                THEN CAST(1 AS bit) ELSE CAST(0 AS bit) END) AS [HasVoted]
            FROM [NewsPolls] p
            WHERE p.[CreatorUserId] = @creatorUserId
              AND (@status IS NULL OR p.[Status] = @status)
            ORDER BY p.[CreatedAt] DESC
            OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY;
            """;
        AddParameter(cmd, "@creatorUserId", userId);
        AddParameter(cmd, "@voterUserId", userId);
        AddParameter(cmd, "@status", status.HasValue ? (byte)status.Value : null, DbType.Byte);
        AddParameter(cmd, "@offset", (page - 1) * pageSize);
        AddParameter(cmd, "@pageSize", pageSize);

        var list = new List<NewsPollListItemDto>();
        await using var reader = await cmd.ExecuteReaderAsync(ct);
        while (await reader.ReadAsync(ct))
        {
            var rawStatus = (NewsPollStatus)reader.GetByte(reader.GetOrdinal("Status"));
            var closesAt = reader.IsDBNull(reader.GetOrdinal("ClosesAt"))
                ? (DateTime?)null
                : reader.GetDateTime(reader.GetOrdinal("ClosesAt"));

            list.Add(new NewsPollListItemDto(
                reader.GetInt32(reader.GetOrdinal("Id")),
                reader.GetString(reader.GetOrdinal("Slug")),
                reader.GetString(reader.GetOrdinal("Title")),
                reader.GetString(reader.GetOrdinal("Question")),
                ToStatusLabel(rawStatus, closesAt),
                reader.GetBoolean(reader.GetOrdinal("AllowMultipleAnswers")),
                reader.GetBoolean(reader.GetOrdinal("ShowResultsBeforeVoting")),
                reader.GetBoolean(reader.GetOrdinal("IsFeatured")),
                NewsPollMediaPath.ToPublicPath(reader.IsDBNull(reader.GetOrdinal("CoverImageUrl"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("CoverImageUrl"))),
                reader.GetInt32(reader.GetOrdinal("OptionCount")),
                reader.GetInt32(reader.GetOrdinal("TotalVotes")),
                reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                closesAt,
                reader.GetBoolean(reader.GetOrdinal("HasVoted"))));
        }

        return list;
    }

    private async Task<int> GetPublishedTotalCountAsync(DbConnection connection, CancellationToken ct)
    {
        await using var cmd = connection.CreateCommand();
        cmd.CommandText = """
            SELECT COUNT(1)
            FROM [NewsPolls]
            WHERE [Status] = @status;
            """;
        AddParameter(cmd, "@status", (byte)NewsPollStatus.Published, DbType.Byte);
        var result = await cmd.ExecuteScalarAsync(ct);
        return result == null ? 0 : Convert.ToInt32(result);
    }

    private async Task<List<NewsPollSummaryDto>> GetPublishedItemsAsync(
        DbConnection connection,
        int? requesterUserId,
        int page,
        int pageSize,
        CancellationToken ct)
    {
        await using var cmd = connection.CreateCommand();
        cmd.CommandText = """
            SELECT p.[Id], p.[Slug], p.[Title], p.[Question], p.[Description], p.[CoverImageUrl], p.[Status],
                   p.[AllowMultipleAnswers], p.[ShowResultsBeforeVoting], p.[CreatedAt], p.[ClosesAt],
                   p.[IsFeatured],
                   (SELECT COUNT(1) FROM [NewsPollVotes] v WHERE v.[PollId] = p.[Id]) AS [TotalVotes],
                   (SELECT CASE WHEN EXISTS(
                        SELECT 1 FROM [NewsPollVotes] v2
                        WHERE v2.[PollId] = p.[Id] AND v2.[VoterUserId] = @viewerUserId
                    ) THEN CAST(1 AS bit) ELSE CAST(0 AS bit) END) AS [HasVoted]
            FROM [NewsPolls] p
            WHERE p.[Status] = @status
            ORDER BY p.[CreatedAt] DESC
            OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY;
            """;
        AddParameter(cmd, "@status", (byte)NewsPollStatus.Published, DbType.Byte);
        AddParameter(cmd, "@viewerUserId", requesterUserId ?? 0);
        AddParameter(cmd, "@offset", (page - 1) * pageSize);
        AddParameter(cmd, "@pageSize", pageSize);

        var list = new List<NewsPollSummaryDto>();
        await using var reader = await cmd.ExecuteReaderAsync(ct);
        while (await reader.ReadAsync(ct))
        {
            var rawStatus = (NewsPollStatus)reader.GetByte(reader.GetOrdinal("Status"));
            var closesAt = reader.IsDBNull(reader.GetOrdinal("ClosesAt"))
                ? (DateTime?)null
                : reader.GetDateTime(reader.GetOrdinal("ClosesAt"));

            list.Add(new NewsPollSummaryDto(
                reader.GetInt32(reader.GetOrdinal("Id")),
                reader.GetString(reader.GetOrdinal("Slug")),
                reader.GetString(reader.GetOrdinal("Title")),
                reader.GetString(reader.GetOrdinal("Question")),
                reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                NewsPollMediaPath.ToPublicPath(reader.IsDBNull(reader.GetOrdinal("CoverImageUrl"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("CoverImageUrl"))),
                ToStatusLabel(rawStatus, closesAt),
                reader.GetBoolean(reader.GetOrdinal("AllowMultipleAnswers")),
                reader.GetBoolean(reader.GetOrdinal("ShowResultsBeforeVoting")),
                reader.GetBoolean(reader.GetOrdinal("IsFeatured")),
                reader.GetInt32(reader.GetOrdinal("TotalVotes")),
                reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                closesAt,
                reader.GetBoolean(reader.GetOrdinal("HasVoted"))));
        }

        return list;
    }

    private async Task<PollRow?> GetPollAsync(int pollId, CancellationToken ct)
    {
        var connection = await GetOpenConnectionAsync(ct);
        await using var cmd = connection.CreateCommand();
        cmd.CommandText = """
            SELECT [Id], [CreatorUserId], [Status], [Title], [Question], [Description], [Slug], [CoverImageUrl], [ClosesAt],
                   [AllowMultipleAnswers], [ShowResultsBeforeVoting], [IsFeatured], [TagsJson], [LocationId], [CreatedAt]
            FROM [NewsPolls]
            WHERE [Id] = @pollId;
            """;
        AddParameter(cmd, "@pollId", pollId);
        PollRow? poll = null;
        await using (var reader = await cmd.ExecuteReaderAsync(ct))
        {
            if (!await reader.ReadAsync(ct))
                return null;

            poll = new PollRow
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                CreatorUserId = reader.GetInt32(reader.GetOrdinal("CreatorUserId")),
                Status = (NewsPollStatus)reader.GetByte(reader.GetOrdinal("Status")),
                Title = reader.GetString(reader.GetOrdinal("Title")),
                Question = reader.GetString(reader.GetOrdinal("Question")),
                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                Slug = reader.GetString(reader.GetOrdinal("Slug")),
                CoverImageUrl = reader.IsDBNull(reader.GetOrdinal("CoverImageUrl")) ? null : reader.GetString(reader.GetOrdinal("CoverImageUrl")),
                ClosesAt = reader.IsDBNull(reader.GetOrdinal("ClosesAt")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("ClosesAt")),
                AllowMultipleAnswers = reader.GetBoolean(reader.GetOrdinal("AllowMultipleAnswers")),
                ShowResultsBeforeVoting = reader.GetBoolean(reader.GetOrdinal("ShowResultsBeforeVoting")),
                IsFeatured = reader.GetBoolean(reader.GetOrdinal("IsFeatured")),
                TagsJson = reader.IsDBNull(reader.GetOrdinal("TagsJson")) ? null : reader.GetString(reader.GetOrdinal("TagsJson")),
                LocationId = reader.IsDBNull(reader.GetOrdinal("LocationId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("LocationId")),
                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
            };
        }

        if (poll == null)
            return null;
        poll.Options = await GetOptionsAsync(pollId, ct);
        return poll;
    }

    private async Task<List<PollOptionRow>> GetOptionsAsync(int pollId, CancellationToken ct)
    {
        var connection = await GetOpenConnectionAsync(ct);
        await using var cmd = connection.CreateCommand();
        cmd.CommandText = """
            SELECT [Id], [Text], [SortOrder]
            FROM [NewsPollOptions]
            WHERE [PollId] = @pollId
            ORDER BY [SortOrder] ASC, [Id] ASC;
            """;
        AddParameter(cmd, "@pollId", pollId);

        var list = new List<PollOptionRow>();
        await using var reader = await cmd.ExecuteReaderAsync(ct);
        while (await reader.ReadAsync(ct))
        {
            list.Add(new PollOptionRow
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Text = reader.GetString(reader.GetOrdinal("Text")),
                SortOrder = reader.GetInt32(reader.GetOrdinal("SortOrder"))
            });
        }

        return list;
    }

    private async Task<List<int>> GetUserSelectedOptionIdsAsync(int pollId, int userId, CancellationToken ct)
    {
        var connection = await GetOpenConnectionAsync(ct);
        await using var cmd = connection.CreateCommand();
        cmd.CommandText = """
            SELECT [SelectedOptionIds]
            FROM [NewsPollVotes]
            WHERE [PollId] = @pollId AND [VoterUserId] = @voterUserId;
            """;
        AddParameter(cmd, "@pollId", pollId);
        AddParameter(cmd, "@voterUserId", userId);
        var result = await cmd.ExecuteScalarAsync(ct);
        return result == null || result == DBNull.Value
            ? []
            : DeserializeIntList(result.ToString());
    }

    private async Task<int> GetTotalVotesAsync(int pollId, CancellationToken ct)
    {
        var connection = await GetOpenConnectionAsync(ct);
        await using var cmd = connection.CreateCommand();
        cmd.CommandText = "SELECT COUNT(1) FROM [NewsPollVotes] WHERE [PollId] = @pollId;";
        AddParameter(cmd, "@pollId", pollId);
        var result = await cmd.ExecuteScalarAsync(ct);
        return result == null ? 0 : Convert.ToInt32(result);
    }

    private async Task<Dictionary<int, int>> GetOptionVoteCountsAsync(int pollId, CancellationToken ct)
    {
        var connection = await GetOpenConnectionAsync(ct);
        await using var cmd = connection.CreateCommand();
        cmd.CommandText = """
            SELECT CAST(j.[value] AS int) AS [OptionId], COUNT(1) AS [Votes]
            FROM [NewsPollVotes] v
            CROSS APPLY OPENJSON(v.[SelectedOptionIds]) j
            WHERE v.[PollId] = @pollId
            GROUP BY CAST(j.[value] AS int);
            """;
        AddParameter(cmd, "@pollId", pollId);

        var map = new Dictionary<int, int>();
        await using var reader = await cmd.ExecuteReaderAsync(ct);
        while (await reader.ReadAsync(ct))
        {
            map[reader.GetInt32(reader.GetOrdinal("OptionId"))] = reader.GetInt32(reader.GetOrdinal("Votes"));
        }

        return map;
    }

    private async Task<int> CountMatchingOptionsAsync(int pollId, IReadOnlyCollection<int> optionIds, CancellationToken ct)
    {
        var connection = await GetOpenConnectionAsync(ct);
        await using var cmd = connection.CreateCommand();

        var parameterNames = optionIds
            .Select((_, i) => $"@optionId{i}")
            .ToList();

        cmd.CommandText = $"""
            SELECT COUNT(1)
            FROM [NewsPollOptions]
            WHERE [PollId] = @pollId
              AND [Id] IN ({string.Join(", ", parameterNames)});
            """;

        AddParameter(cmd, "@pollId", pollId);
        var i = 0;
        foreach (var optionId in optionIds)
        {
            AddParameter(cmd, $"@optionId{i}", optionId);
            i++;
        }

        var result = await cmd.ExecuteScalarAsync(ct);
        return result == null ? 0 : Convert.ToInt32(result);
    }

    private async Task InsertVoteAsync(int pollId, int userId, IReadOnlyCollection<int> optionIds, CancellationToken ct)
    {
        var connection = await GetOpenConnectionAsync(ct);
        await using var cmd = connection.CreateCommand();
        cmd.CommandText = """
            INSERT INTO [NewsPollVotes] ([PollId], [VoterUserId], [SelectedOptionIds], [CreatedAt])
            VALUES (@pollId, @voterUserId, @selectedOptionIds, @createdAt);
            """;
        AddParameter(cmd, "@pollId", pollId);
        AddParameter(cmd, "@voterUserId", userId);
        AddParameter(cmd, "@selectedOptionIds", JsonSerializer.Serialize(optionIds));
        AddParameter(cmd, "@createdAt", DateTime.UtcNow);
        await cmd.ExecuteNonQueryAsync(ct);
    }

    private async Task<int> GetPendingTotalCountAsync(DbConnection connection, string? search, CancellationToken ct)
    {
        await using var cmd = connection.CreateCommand();
        cmd.CommandText = """
            SELECT COUNT(1)
            FROM [NewsPolls] p
            WHERE p.[Status] = @status
              AND (@search IS NULL OR p.[Title] LIKE '%' + @search + '%' OR p.[Question] LIKE '%' + @search + '%');
            """;
        AddParameter(cmd, "@status", (byte)NewsPollStatus.PendingReview);
        AddParameter(cmd, "@search", search, DbType.String);
        var result = await cmd.ExecuteScalarAsync(ct);
        return result == null ? 0 : Convert.ToInt32(result);
    }

    private async Task<List<NewsPollAdminPendingDto>> GetPendingItemsAsync(
        DbConnection connection,
        string? search,
        int page,
        int pageSize,
        CancellationToken ct)
    {
        await using var cmd = connection.CreateCommand();
        cmd.CommandText = """
            SELECT p.[Id], p.[Slug], p.[Title], p.[Question], p.[Description], p.[CoverImageUrl], p.[CreatorUserId], p.[CreatedAt],
                   p.[ClosesAt], p.[AllowMultipleAnswers], p.[ShowResultsBeforeVoting],
                   CONCAT(COALESCE(u.[FirstName], ''), CASE WHEN u.[LastName] IS NULL OR u.[LastName] = '' THEN '' ELSE ' ' + u.[LastName] END) AS [CreatorName],
                   (SELECT COUNT(1) FROM [NewsPollOptions] o WHERE o.[PollId] = p.[Id]) AS [OptionCount]
            FROM [NewsPolls] p
            INNER JOIN [UserProfiles] u ON u.[UserId] = p.[CreatorUserId]
            WHERE p.[Status] = @status
              AND (@search IS NULL OR p.[Title] LIKE '%' + @search + '%' OR p.[Question] LIKE '%' + @search + '%')
            ORDER BY p.[CreatedAt] DESC
            OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY;
            """;
        AddParameter(cmd, "@status", (byte)NewsPollStatus.PendingReview);
        AddParameter(cmd, "@search", search, DbType.String);
        AddParameter(cmd, "@offset", (page - 1) * pageSize);
        AddParameter(cmd, "@pageSize", pageSize);

        var items = new List<NewsPollAdminPendingDto>();
        await using var reader = await cmd.ExecuteReaderAsync(ct);
        while (await reader.ReadAsync(ct))
        {
            items.Add(new NewsPollAdminPendingDto(
                reader.GetInt32(reader.GetOrdinal("Id")),
                reader.GetString(reader.GetOrdinal("Slug")),
                reader.GetString(reader.GetOrdinal("Title")),
                reader.GetString(reader.GetOrdinal("Question")),
                reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                NewsPollMediaPath.ToPublicPath(reader.IsDBNull(reader.GetOrdinal("CoverImageUrl"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("CoverImageUrl"))),
                reader.GetInt32(reader.GetOrdinal("CreatorUserId")),
                reader.GetString(reader.GetOrdinal("CreatorName")).Trim(),
                reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                reader.IsDBNull(reader.GetOrdinal("ClosesAt")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("ClosesAt")),
                reader.GetBoolean(reader.GetOrdinal("AllowMultipleAnswers")),
                reader.GetBoolean(reader.GetOrdinal("ShowResultsBeforeVoting")),
                reader.GetInt32(reader.GetOrdinal("OptionCount"))));
        }

        return items;
    }

    private sealed class PollRow
    {
        public int Id { get; set; }
        public int CreatorUserId { get; set; }
        public NewsPollStatus Status { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Question { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string? CoverImageUrl { get; set; }
        public DateTime? ClosesAt { get; set; }
        public bool AllowMultipleAnswers { get; set; }
        public bool ShowResultsBeforeVoting { get; set; }
        public bool IsFeatured { get; set; }
        public string? TagsJson { get; set; }
        public int? LocationId { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<PollOptionRow> Options { get; set; } = [];
    }

    private sealed class PollOptionRow
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public int SortOrder { get; set; }
    }
}
