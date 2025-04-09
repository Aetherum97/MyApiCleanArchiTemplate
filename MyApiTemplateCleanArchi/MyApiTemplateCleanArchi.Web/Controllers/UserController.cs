using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyApiTemplateCleanArchi.Application.DTOs;
using MyApiTemplateCleanArchi.Domain.Interfaces;
using MyApiTemplateCleanArchi.Shared.Commons.Pagination;

namespace MyApiTemplateCleanArchi.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var userDto = _mapper.Map<UserDto>(user);
            return Ok(userDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers([FromQuery] PaginationParameters parameters)
        {
            var pagedUsers = await _userRepository.GetAllUsersAsync(parameters);
            var userDtos = _mapper.Map<PagedList<UserDto>>(pagedUsers);
            return Ok(userDtos);
        }
    }
}
