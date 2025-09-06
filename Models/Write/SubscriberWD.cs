namespace Core.Models.Write
{
    public class SubscriberWD
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long SubscriberId { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsActive { get; set; }
    }
}
