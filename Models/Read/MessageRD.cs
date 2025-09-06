namespace Core.Models.Read
{
    public class MessageRD
    {
        public long Id { get; set; }
        public long SenderId { get; set; }
        public string? SenderUsername { get; set; }
        public long ReceiverId { get; set; }
        public string? ReceiverUsername { get; set; }
        public DateTime SendDate { get; set; }
        public bool IsRead { get; set; }
        public string? Message1 { get; set; }

        public string? FileUrl { get; set; }
    }
}
