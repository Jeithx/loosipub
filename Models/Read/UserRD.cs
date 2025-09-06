namespace Core.Models.Read
{
    public class UserRD
    {
        public long Id { get; set; }
        public int GenderId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int UserTypeId { get; set; }
        public int Status { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastLoginAt { get; set; }
        public bool IsActive { get; set; }
        public string? Biography { get; set; }

        public long CountryId { get; set; }
        public long CityId { get; set; }
        public int PrefferedLanguageId { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? DisplayName { get; set; }
        public string? ProfilePictureUrl { get; set; }

        public bool IsLocked { get; set; }

        public bool? IsFollowed { get; set; } = false;
        public string? CoverPictureUrl { get; set; }
        public virtual GenderRD Gender { get; set; } = null!;
        public virtual CountryRD Country { get; set; } = null!;
        public virtual CityRD City { get; set; } = null!;
    }
}
