using UserAuthApiPg.Data;
using UserAuthApiPg.Models;
using UserAuthApiPg.Models.DTOs;
using Microsoft.AspNetCore.Authorization; // Add this
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace UserAuthApiPg.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(AuthDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // Existing endpoints (register, login, etc.) remain unchanged

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (await _context.Users.AnyAsync(u => u.Username == registerDto.Username || u.Email == registerDto.Email))
            {
                return BadRequest("Username or email already exists.");
            }

            var user = new User
            {
                Username = registerDto.Username,
                Email = registerDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                Role = registerDto.Role ?? UserRole.Employee // Default to User if not specified
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "User registered successfully." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                return Unauthorized("Invalid credentials");

            var session = new Session
            {
                UserId = user.Id,
                SessionToken = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddHours(24),
                IsActive = true
            };

            _context.Sessions.Add(session);
            await _context.SaveChangesAsync();

            var token = GenerateJwtToken(user, session.SessionToken);
            return Ok(new { token, sessionId = session.Id });
        }



        [HttpGet("check-sessions/{userId}")]
        public async Task<IActionResult> CheckSessions(int userId)
        {
            var sessions = await _context.Sessions
                .Where(s => s.UserId == userId && s.IsActive)
                .ToListAsync();
            return Ok(sessions);
        }

        [HttpPost("clear-session")]
        public async Task<IActionResult> ClearSession([FromBody] int sessionId)
        {
            var session = await _context.Sessions.FindAsync(sessionId);
            if (session == null || !session.IsActive)
                return BadRequest("Session not found or already inactive");

            session.IsActive = false;
            session.ExpiresAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Session cleared" });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] int sessionId)
        {
            return await ClearSession(sessionId);
        }

        [HttpGet("users")]
        [Authorize(Policy = "AdminOnly")] // Restrict to Admin role
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _context.Users
                .Select(u => new
                {
                    u.Id,
                    u.Username,
                    u.Email,
                    u.Role
                })
                .ToListAsync();
            return Ok(users);
        }

        private string GenerateJwtToken(User user, string sessionToken)
        {
            var claims = new[]
            {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Role, user.Role.ToString()), // Convert enum to string for JWT
        new Claim("SessionToken", sessionToken)
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1), // Adjust expiration as needed
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost("admin")]
        [Authorize(Policy = "AdminOnly")] // Only admins can create new admins
        public async Task<IActionResult> CreateAdmin([FromBody] CreateAdminDto adminDto)
        {
            if (await _context.Users.AnyAsync(u => u.Username == adminDto.Username))
                return BadRequest("Username already exists.");

            if (await _context.Users.AnyAsync(u => u.Email == adminDto.Email))
                return BadRequest("Email already exists.");

            var user = new User
            {
                Username = adminDto.Username,
                Email = adminDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(adminDto.Password),
                Role = adminDto.Role ?? UserRole.Admin // New user is an adminz
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Admin created successfully.", userId = user.Id });
        }

        [HttpDelete("users/{userId}")]
        [Authorize(Policy = "AdminOnly")] // Only admins can remove users/admins
        public async Task<IActionResult> RemoveUser(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound("User not found.");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "User removed successfully." });
        }

        // [HttpPut("users/{userId}")]
        // [Authorize(Policy = "AdminOnly")] // Only admins can edit users/admins
        // public async Task<IActionResult> UpdateUser(int userId, [FromBody] UpdateUserDto updateDto)
        // {
        //     var user = await _context.Users.FindAsync(userId);
        //     if (user == null)
        //         return NotFound("User not found.");

        //     if (!string.IsNullOrEmpty(updateDto.Username))
        //         user.Username = updateDto.Username;
        //     if (!string.IsNullOrEmpty(updateDto.Email))
        //         user.Email = updateDto.Email;
        //     if (!string.IsNullOrEmpty(updateDto.Role))
        //     {
        //         if (updateDto.Role != UserRole.Employee && updateDto.Role != UserRole.admin)
        //             return BadRequest("Invalid role. Use 'User' or 'Admin'.");
        //         user.Role = updateDto.Role;
        //     }
        //     if (!string.IsNullOrEmpty(updateDto.Password))
        //         user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(updateDto.Password);

        //     await _context.SaveChangesAsync();

        //     return Ok(new { message = "User updated successfully." });
        // }

        [HttpPut("users/{userId}")]
        [Authorize(Policy = "AdminOrOwner")] // Restrict to Admin or Owner
        public async Task<IActionResult> UpdateUser(int userId, [FromBody] UpdateUserDto updateDto)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            if (!string.IsNullOrEmpty(updateDto.Username))
            {
                user.Username = updateDto.Username;
            }
            if (!string.IsNullOrEmpty(updateDto.Email))
            {
                user.Email = updateDto.Email;
            }
            if (updateDto.Role.ToString() != null) // Check if Role is not null (enum is nullable)
            {
                user.Role = updateDto.Role; // Use the enum value
            }
            if (!string.IsNullOrEmpty(updateDto.Password))
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(updateDto.Password);
            }

            await _context.SaveChangesAsync();

            return Ok(new { message = "User updated successfully." });
        }
    }
}