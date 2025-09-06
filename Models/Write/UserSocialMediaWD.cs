namespace Core.Models.Write
{
    public class UserSocialMediaWD
    {
        public long Id { get; set; }

        public long SocialMediaId { get; set; }

        public long UserId { get; set; }
        public string Url { get; set; } = null!;

        public DateTime CreationDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public bool IsActive { get; set; }
    }
}
