namespace Core.Models.Read
{
    public class PostRD
    {
        public long Id { get; set; }
        public string? Title { get; set; }
        public long UserId { get; set; }
        public int MediaType { get; set; }
        public int Visibility { get; set; }
        public decimal? Price { get; set; }
        public bool IsPinned { get; set; }
        public bool IsScheduled { get; set; }
        public bool IsNsfw { get; set; }
        public int LikeCount { get; set; }
        public DateTime? ScheduledFor { get; set; }
        public int CommentCount { get; set; }
        public int ViewCount { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsActive { get; set; }
        public int TipCount { get; set; }
        public int Type { get; set; }

        public string UserDisplayName { get; set; }
        public string UserName { get; set; }
        public string UserProfilePicture { get; set; }

        public bool CurrentUserLiked { get; set; } = false;

        public virtual List<PostMediaRD>? PostMedia { get; set; }
        public virtual List<PostTagRD>? PostTags { get; set; }

        public virtual ICollection<PostCommentRD>? PostComments { get; set; } = new List<PostCommentRD>();
    }
}
