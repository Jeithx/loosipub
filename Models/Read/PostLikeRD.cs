namespace Core.Models.Read
{
    public class PostLikeRD
    {
        public long UserId { get; set; }
        public long PostId { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsActive { get; set; }
        public bool? FollowedUserCheck { get; set; }

        public PostRD Post { get; set; }
        public virtual UserRD User { get; set; }
    }
}
