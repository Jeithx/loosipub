namespace Core.Models.Read
{
    public class SubscriberRD
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long SubscriberId { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsActive { get; set; }

        public virtual UserRD? SubscriberNavigation { get; set; }
        public virtual UserRD? User { get; set; }
    }
}
