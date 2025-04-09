using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyApiTemplateCleanArchi.Application.DTOs.Auth;
using MyApiTemplateCleanArchi.Application.Services.Interfaces;
using MyApiTemplateCleanArchi.Domain.Entities;
using MyApiTemplateCleanArchi.Infrastructure.Persistence;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyApiTemplateCleanArchi.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        private readonly ITokenService _tokenService;

        public AuthController(IConfiguration configuration, ApplicationDbContext context, ITokenService tokenService)
        {
            _configuration = configuration;
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            try
            {
                var user = _context.Users.SingleOrDefault(u => u.Username == model.Username);
                if (user == null)
                {
                    return Unauthorized("Identifiants invalides");
                }

                var token = _tokenService.GenerateToken(user);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur lors de la connexion: {ex.Message}");
            }
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterModel model)
        {
            try
            {
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

                var newUser = new User
                {
                    Username = model.Username,
                    Password = hashedPassword
                };

                _context.Users.Add(newUser);
                _context.SaveChanges();

                return Ok("Utilisateur créé");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur lors de l'inscription: {ex.Message}");
            }
        }
    }
}
