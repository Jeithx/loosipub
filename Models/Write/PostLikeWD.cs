namespace Core.Models.Write
{
    public class PostLikeWD
    {
        public long UserId { get; set; }
        public long PostId { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsActive { get; set; }
    }
}
