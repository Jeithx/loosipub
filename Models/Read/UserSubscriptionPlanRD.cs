namespace Core.Models.Read
{
    public class UserSubscriptionPlanRD
    {
        public long Id { get; set; }

        public string Username { get; set; }
        public string DisplayName { get; set; }
        public long ContentCreatorUserId { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public string CurrencyCode { get; set; } = null!;

        public int DurationInDays { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime? UpdateDate { get; set; }

    }
}
