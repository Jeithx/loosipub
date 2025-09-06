namespace Core.Models.Read
{
    public class PostCommentRD
    {
        public long Id { get; set; }
        public long PostId { get; set; }
        public long UserId { get; set; }
        public string Comments { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsActive { get; set; }
        public virtual UserRD User { get; set; } = null!;

    }
}
