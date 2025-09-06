namespace Core.Models.Write
{
    public class PostWD
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
        

        public virtual List<PostTagWD>? PostTags { get; set; }
        public virtual List<PostMediaWD>? PostMedias { get; set; }
    }

    public class PostUpdateWD
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public int MediaType { get; set; }
        public int Visibility { get; set; }
        public bool IsPinned { get; set; }
        public bool IsScheduled { get; set; }
        public bool IsNsfw { get; set; }
        public bool IsActive { get; set; }
        public int Type { get; set; }
    }
}
