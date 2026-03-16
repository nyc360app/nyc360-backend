namespace NYC360.API.Models.Tags;

public record RemoveUserTagRequest(
    int UserId,
    int TagId
);