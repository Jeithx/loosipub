namespace Core.Models.Read
{
    public class CallRD
    {
        public long Id { get; set; }

        public long CallerUserId { get; set; }
        public string? CallerUsername { get; set; }
        public string? CalledDisplayname { get; set; }


        public long ReceiverUserId { get; set; }

        public string? ReceiverUsername { get; set; }
        public string? ReceiverDisplayname { get; set; }


        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public int? TotalDurationInSeconds { get; set; }

        public decimal? TotalPrice { get; set; }

        public int Currency { get; set; }

        public int Status { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public bool IsActive { get; set; }
    }
}
