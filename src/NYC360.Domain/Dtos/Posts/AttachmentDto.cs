using NYC360.Domain.Entities.Housing;
using NYC360.Domain.Entities.Posts;

namespace NYC360.Domain.Dtos.Posts;

public record AttachmentDto(int Id, string Url);

public static class AttachmentDtoExtensions
{
    extension(AttachmentDto)
    {
        public static AttachmentDto Map(PostAttachment postAttachment)
        {
            return new AttachmentDto(postAttachment.Id, postAttachment.Url);
        }
        public static AttachmentDto Map(HousingAttachment postAttachment)
        {
            return new AttachmentDto(postAttachment.Id, postAttachment.Url);
        }
        public static AttachmentDto Map(HouseListingAuthorizationAttachment attachment)
        {
            return new AttachmentDto(attachment.Id, attachment.Url);
        }
    }
}