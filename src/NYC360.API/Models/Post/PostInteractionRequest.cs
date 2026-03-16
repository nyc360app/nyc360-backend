using NYC360.Domain.Enums;
using NYC360.Domain.Enums.Posts;

namespace NYC360.API.Models.Post;

public record PostInteractionRequest(int PostId, InteractionType Type);