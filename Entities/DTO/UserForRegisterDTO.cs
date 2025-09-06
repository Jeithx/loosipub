namespace Core.Entities.DTO
{
    public partial class UserForRegisterDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Phone { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public DateTime BirthDate { get; set; }
        public int GenderId { get; set; }
        public long CountryId { get; set; }
        public long CityId { get; set; }
        public int PrefferedLanguageId { get; set; }
    }
}
