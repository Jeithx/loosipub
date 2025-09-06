namespace Core.Models.Write
{
    public class BlockedUserWD
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long BlockedUserId { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsActive { get; set; }
    }
}
