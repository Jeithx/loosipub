namespace Core.Models.Write
{
    public class PostTagWD
    {
        public long Id { get; set; }
        public long PostId { get; set; }
        public long? TagId { get; set; }

        public string Tag { get; set; }
    }
}
