namespace Core.Models.Read
{
    public class PostMediaRD
    {
        public long Id { get; set; }
        public long PostId { get; set; }
        public string MediaUrl { get; set; }
        public string? BlurredUrl { get; set; }
        public string? ThumbnailUrl { get; set; }
        public int Type { get; set; }
        public int Order { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsActive { get; set; }
    }
}
