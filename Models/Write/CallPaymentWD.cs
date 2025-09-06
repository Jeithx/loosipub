namespace Core.Models.Write
{
    public class CallPaymentWD
    {
        public long Id { get; set; }

        public long CallId { get; set; }

        public long PayerId { get; set; }

        public decimal Amount { get; set; }

        public DateTime PaidDate { get; set; }

        public int PaymentStatus { get; set; }

    }
}
