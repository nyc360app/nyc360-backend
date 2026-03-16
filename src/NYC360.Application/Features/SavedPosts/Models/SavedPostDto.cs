namespace NYC360.Application.Features.SavedPosts.Models
{
    public class SavedPostDto
    {
        public int Id { get; set; } // Id from UserSavedPost
        public int PostId { get; set; }
        public string? Title { get; set; }
        public int? AuthorId { get; set; }
        public string? AuthorUsername { get; set; }
        public DateTime PostCreatedAt { get; set; } // Original Post creation date
        public DateTime SavedAt { get; set; } // When the post was saved (from UserSavedPost)
    }
}
