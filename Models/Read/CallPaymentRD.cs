namespace Core.Models.Read
{
    public class CallPaymentRD
    {
        public long Id { get; set; }

        public long CallId { get; set; }
        public string? CalledUsername { get; set; }
        public string? CalledUserDisplayname { get; set; }

        public long PayerId { get; set; }

        public decimal Amount { get; set; }

        public DateTime PaidDate { get; set; }

        public int PaymentStatus { get; set; }
    }
}
