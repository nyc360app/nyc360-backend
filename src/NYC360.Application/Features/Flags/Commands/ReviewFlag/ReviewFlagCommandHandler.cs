using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Enums;
using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Flags.Commands.ReviewFlag;

public class ReviewFlagCommandHandler(
    IPostFlagRepository postFlagRepository,
    IPostRepository postRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<ReviewFlagCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(ReviewFlagCommand request, CancellationToken ct)
    {
        if (request.NewStatus != FlagStatus.Rejected && request.NewStatus != FlagStatus.ActionTaken)
        {
            var error = new ApiError("flag.invalid_status", "Flag can only be marked as Rejected or ActionTaken.");
            return StandardResponse.Failure(error);
        }

        var flag = await postFlagRepository.GetByIdAsync(request.FlagId, ct); 
        if (flag is null)
        {
            return StandardResponse.Failure(new("flag.not_found", $"Flag with ID {request.FlagId} not found."));
        }
        
        flag.Status = request.NewStatus;
        flag.ReviewedAt = DateTime.UtcNow;
        flag.ReviewerId = request.UserId;
        flag.ReviewerNote = request.AdminNote;
        
        postFlagRepository.Update(flag);

        if (flag.Status == FlagStatus.ActionTaken)
        {
            var post = await postRepository.GetByIdAsync(flag.PostId, ct);
            postRepository.Remove(post);
        }
        
        await unitOfWork.SaveChangesAsync(ct);
        
        return StandardResponse.Success();
    }
}