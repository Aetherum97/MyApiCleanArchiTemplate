using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApiTemplateCleanArchi.Domain.Entities;
using MyApiTemplateCleanArchi.Infrastructure.Persistence.DbContexts;

namespace MyApiTemplateCleanArchi.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodosController : ControllerBase
    {
        private readonly TodoPostgreDbContext _db;

        public TodosController(TodoPostgreDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _db.Todos.ToListAsync();
            return Ok(list);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOne(int id)
        {
            var todo = await _db.Todos.FindAsync(id);
            if (todo == null)
                return NotFound();
            return Ok(todo);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Todo input)
        {
            _db.Todos.Add(input);
            await _db.SaveChangesAsync();
            return CreatedAtAction(
                nameof(GetOne),
                new { id = input.Id },
                input
            );
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Todo input)
        {
            if (id != input.Id)
                return BadRequest("L’ID de l’URL doit correspondre à l’ID de l’objet.");

            var exists = await _db.Todos.AnyAsync(x => x.Id == id);
            if (!exists)
                return NotFound();

            _db.Entry(input).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var todo = await _db.Todos.FindAsync(id);
            if (todo == null)
                return NotFound();

            _db.Todos.Remove(todo);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
