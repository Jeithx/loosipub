namespace Core.Models.Read
{
    public class UserCallPriceRD
    {
        public long Id { get; set; }

        public long UserId { get; set; }
        public string? Username { get; set; }
        public string? DisplayName { get; set; }
        public string? ProfilePhotoUrl { get; set; }
        public string? CoverPhotoUrl { get; set; }

        public decimal RatePerMinuute { get; set; }

        public int Currency { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime? UpdateDate { get; set; }
    }
}
