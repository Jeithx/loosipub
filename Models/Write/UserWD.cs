namespace Core.Models.Write
{
    public class UserWD
    {
        public long Id { get; set; }
        public int GenderId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int UserTypeId { get; set; }
        public int Status { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime LastLoginAt { get; set; }
        public string? Biography { get; set; }

        public bool IsActive { get; set; }
        public long CountryId { get; set; }
        public long CityId { get; set; }
        public int PrefferedLanguageId { get; set; }
        public string? ProfilePictureUrl { get; set; }

        public bool IsLocked { get; set; } = false;
        public string? CoverPictureUrl { get; set; }
        public string? DisplayName { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? BirthDate { get; set; }
        public byte[]? PasswordHash { get; set; } = null!;

        public byte[]? PasswordSalt { get; set; } = null!;
        public bool? IsVerified { get; set; }

    }

    public class ChangePassword
    {
        public long Id { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string NewPasswordRepeat { get; set; }
    }
}
