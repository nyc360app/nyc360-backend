using NYC360.Application.Contracts.Persistence;
using Microsoft.AspNetCore.Identity;
using NYC360.Domain.Entities.Posts;
using NYC360.Domain.Entities.User;
using NYC360.Domain.Dtos.Posts;
using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.PostComments.Commands.Create;

public class CreatePostCommentCommandHandler(
    IPostCommentRepository postCommentRepository, 
    IPostRepository postRepository,
    IUnitOfWork unitOfWork,
    IUserRepository userRepository)
    : IRequestHandler<CreatePostCommentCommand, StandardResponse<PostCommentDto>>
{
    public async Task<StandardResponse<PostCommentDto>> Handle(CreatePostCommentCommand request, CancellationToken ct)
        {
            var post = await postRepository.GetByIdAsync(request.PostId, ct);
            if (post is null)
                return StandardResponse<PostCommentDto>.Failure(new("post.notfound", "Post not found."));

            var user = await userRepository.GetByIdWithStatsAsync(request.UserId, ct);
            if (user is null)
                return StandardResponse<PostCommentDto>.Failure(new("auth.notfound", "User not found."));

            
            if (request.ParentCommentId != null && request.ParentCommentId.Value != 0)
            {
                var parentComment = await postCommentRepository.GetCommentByIdAsync(request.ParentCommentId.Value, ct);
                if (parentComment is null)
                    return StandardResponse<PostCommentDto>.Failure(new ApiError("comment.notfound", "Parent comment not found."));
            }
            
            var comment = new PostComment
            {
                PostId = request.PostId,
                UserId = request.UserId,
                ParentCommentId = request.ParentCommentId == 0 ? null : request.ParentCommentId,
                Content = request.Content,
                CreatedAt = DateTime.UtcNow,
            };

            await postCommentRepository.AddCommentAsync(comment, ct);
            await unitOfWork.SaveChangesAsync(ct);
            
            return StandardResponse<PostCommentDto>.Success(PostCommentDto.Map(comment));
        }
    }