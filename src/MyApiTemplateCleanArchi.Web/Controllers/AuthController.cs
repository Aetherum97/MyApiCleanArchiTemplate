using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyApiTemplateCleanArchi.Application.Commands.Auth;
using MyApiTemplateCleanArchi.Application.DTOs.Auth;
using MyApiTemplateCleanArchi.Application.Modules.Interfaces;
using MyApiTemplateCleanArchi.Application.Services.Interfaces;
using MyApiTemplateCleanArchi.Domain.Entities;
using MyApiTemplateCleanArchi.Infrastructure.Persistence;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace MyApiTemplateCleanArchi.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            AuthResponseDto result = await _mediator.Send(command);

            if (result == null)
            {
                return Unauthorized("Identifiants invalides");
            }

            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCommand command)
        {
            string message = await _mediator.Send(command);
            return Ok(new { Message = message });
        }

    }
}
