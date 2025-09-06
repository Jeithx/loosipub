namespace Core.Models.Read
{
    public class PostTagRD
    {
        public long Id { get; set; }
        public long PostId { get; set; }
        public long TagId { get; set; }

        public string? Tag { get; set; }
    }
}
