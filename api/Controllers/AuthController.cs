using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api.Models;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;

        public AuthController(IConfiguration configuration, AppDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterDto register)
        {
            var existingUser = _context.Users
                .FirstOrDefault(u => u.Username == register.Username);

            if (existingUser != null)
            {
                return BadRequest("Username already exists");
            }

            var user = new User
            {
                Username = register.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(register.Password),
                Role = "User",
                Email = register.Email,
                CreatedTime = DateTime.UtcNow,
                favoriteMotivator = register.favoriteMotivator
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok("User created successfully");
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto login)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Username == login.Username);

            if (user != null && BCrypt.Net.BCrypt.Verify(login.Password, user.PasswordHash))
            {
                var token = GenerateJwtToken(user);
                return Ok(new { token, role = user.Role, userId = user.Id, favoriteMotivator = user.favoriteMotivator});
            }

            return Unauthorized("Invalid username or password");
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class RegisterDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string? favoriteMotivator {get; set; }
    }

    public class LoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}