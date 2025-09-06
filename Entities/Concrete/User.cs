namespace Core.Entities.Concrete
{
    public partial class User : IEntity
    {
        public long Id { get; set; }

        public string? Username { get; set; }
        public string? DisplayName { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public long? GenderId { get; set; }
        public bool? ResetPassword { get; set; }
        public int? UserTypeId { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
    }
}
