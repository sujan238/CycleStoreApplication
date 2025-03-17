// RegisterDto.cs
namespace UserAuthApiPg.Models.DTOs
{
    public class RegisterDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public UserRole? Role { get; set; }
    }
}