using System.ComponentModel.DataAnnotations;

namespace UserAuthApiPg.Models.DTOs
{
    public class UpdateUserDto
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        [Required]
        public UserRole Role { get; set; } // Only admin can change role
        public string? Password { get; set; } // Optional for password update
    }
}