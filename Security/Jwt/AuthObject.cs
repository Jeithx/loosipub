namespace Core.Utilities.Security.Jwt
{
    public class AuthObject
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool? ResetPassword { get; set; }
        public int? GenderId { get; set; }
        public string[] Roles { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string ProfilePhoto { get; set; }
        public List<ClaimTypeVM> Claims { get; set; }
    }
}
