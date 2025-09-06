namespace Core.Models.Write
{
    public class NotificationWD
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public int Type { get; set; }

        public string Content { get; set; } = null!;

        public bool IsRead { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public bool IsActive { get; set; }
    }
}
