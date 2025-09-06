namespace Core.Models.Write
{
    public class UserFollowerWD
    {
        public long Id { get; set; }

        public long FollowerId { get; set; }

        public long FollowedId { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public bool IsActive { get; set; }
    }
}
