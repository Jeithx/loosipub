namespace Core.Models.Write
{
    public class ReportWD
    {
        public long Id { get; set; }
        public long ReporterId { get; set; }

        public long? ReportedUserId { get; set; }

        public long? PostId { get; set; }

        public string Reason { get; set; } = null!;

        public DateTime CreationDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public bool IsActive { get; set; }
    }
}
