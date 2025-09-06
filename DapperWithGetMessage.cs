namespace Entities.Dtos
{
    public class DapperWithGetMessage
    {
        public long SenderId { get; set; }
        public long ReceiverId { get; set; }
        public long MessageCount { get; set; }
        public DateTime FirstMessage { get; set; }
        public DateTime LastMessage { get; set; }
        public string DisplayName { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string? LastMessageContent { get; set; }
    }
}
