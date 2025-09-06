namespace Core.Models.Write
{
    public class UserLoginAttemptWD
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public int Step { get; set; }

        public int Status { get; set; }

        public string? Ipaddress { get; set; }

        public string? UserAgent { get; set; }

        public DateTime AttemptDate { get; set; }

        public string AdditionalData { get; set; } = null!;
    }
}
