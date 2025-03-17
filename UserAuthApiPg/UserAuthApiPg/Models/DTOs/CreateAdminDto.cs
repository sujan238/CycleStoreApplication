namespace UserAuthApiPg.Models.DTOs
{
    public class CreateAdminDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserRole? Role { get; set; }
    }
}