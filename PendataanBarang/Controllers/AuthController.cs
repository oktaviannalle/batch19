using Microsoft.AspNetCore.Mvc;
using PendataanBarang.Data;
using PendataanBarang.DTOs;
using PendataanBarang.Helpers;
using PendataanBarang.Models;

namespace PendataanBarang.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly JwtTokenGenerator _jwt;

        public AuthController(AppDbContext context, JwtTokenGenerator jwt)
        {
            _context = context;
            _jwt = jwt;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO dto)
        {
            if (_context.Users.Any(u => u.Username == dto.Username))
                return Conflict("Username sudah digunakan.");

            var user = new User
            {
                Username = dto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok("Registrasi berhasil.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == dto.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return Unauthorized("Username atau password salah.");

            var token = _jwt.GenerateToken(user);
            return Ok(new { Token = token });
        }
    }
}