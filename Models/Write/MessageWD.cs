using Microsoft.AspNetCore.Http;

namespace Core.Models.Write
{
    public class MessageWD
    {
        public long Id { get; set; }
        public long? SenderId { get; set; }
        public long ReceiverId { get; set; }
        public DateTime SendDate { get; set; }
        public bool IsRead { get; set; }
        public string? Message1 { get; set; }
        public IFormFile? File { get; set; }
        public string? FileUrl { get; set; }
    }
}
