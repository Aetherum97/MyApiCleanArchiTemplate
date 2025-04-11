using Microsoft.AspNetCore.Mvc;
using MyApiTemplateCleanArchi.Application.Modules.Interfaces;
using MyApiTemplateCleanArchi.Application.Queries.GetAllUsers;
using MyApiTemplateCleanArchi.Application.Queries.GetUser;
using MyApiTemplateCleanArchi.Shared.Commons.Pagination;

namespace MyApiTemplateCleanArchi.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var query = new GetUserQuery(id);
            var result = await _mediator.Send(query);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers([FromQuery] PaginationParameters parameters)
        {
            var query = new GetAllUsersQuery(parameters);

            var result = await _mediator.Send(query);

            return Ok(result);
        }
    }
}
