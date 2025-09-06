namespace Core.Models.Read
{
    public class UserSocialMediaRD
    {
        public long Id { get; set; }
        public long SocialMediaId { get; set; }
        public long UserId { get; set; }
        public string Url { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsActive { get; set; }

        public string? SocialMediaName { get; set; }
        public string? SocialMediaLogo { get; set; }
    }
}
