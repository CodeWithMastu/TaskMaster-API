using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskMaster.DTOs;
using TaskMaster.Models;
using TaskMaster.Services;

namespace TaskMaster.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController: ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly AuthService _auth;

        public AuthController(AppDbContext context, AuthService auth)
        {
            _context = context;
            _auth = auth;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (_context.Users.Any(u => u.Email == dto.Email))
                return BadRequest("Email already in use");

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = _auth.HashPassword(dto.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "User registered" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null || !_auth.VerifyPassword(dto.Password, user.PasswordHash))
                return Unauthorized("Invalid credentials");

            var token = _auth.GenerateJwtToken(user);
            return Ok(new { token });
        }
    }
}
