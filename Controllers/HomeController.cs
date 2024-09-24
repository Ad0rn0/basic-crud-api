using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Todo.Models;
using Todo.Data;

namespace Todo.Controllers
{
	[ApiController]
	public class HomeController : ControllerBase
	{
		[HttpGet("/")]
		public async Task<IActionResult> GetAllAsync([FromServices] AppDbContext dbContext)
			 => Ok(await dbContext.Todos.ToListAsync());

		[HttpPost("/")]
		public async Task<IActionResult> PostAsync(
			[FromBody] TodoModel todo,
			[FromServices] AppDbContext dbContext
		)
		{
			dbContext.Todos.Add(todo);
			await dbContext.SaveChangesAsync();

			return Created($"/{todo.Id}", todo);
		}

		[HttpGet("/{id:int}")]
		public async Task<IActionResult> GetByIdAsync(
			[FromRoute] int id,
			[FromServices] AppDbContext context)
		{
			var todo = await context.Todos.FirstOrDefaultAsync(t => t.Id == id);

			if (todo == null)
			{
				return NotFound($"Id {id} não encontrado.");
			}

			return Ok(todo);
		}

		[HttpPut("/{id:int}")]
		public async Task<IActionResult> PutAsync(
			[FromRoute] int id,
			[FromBody] TodoModel todo,
			[FromServices] AppDbContext context)
		{
			var todoToUpdate = await context.Todos.FirstOrDefaultAsync(t => t.Id == id);

			if (todoToUpdate == null)
			{
				return NotFound($"Id {id} não encontrado");
			}

			todoToUpdate.Title = todo.Title;
			todoToUpdate.Done = todo.Done;

			context.Todos.Update(todoToUpdate);
			context.SaveChanges();

			return Ok(todoToUpdate);
		}

		[HttpDelete("/{id:int}")]
		public async Task<IActionResult> Delete(
			[FromRoute] int id,
			[FromServices] AppDbContext context
		)
		{
			var todo = await context.Todos.FirstOrDefaultAsync(t => t.Id == id);

			if (todo == null)
				return NotFound($"Id {id} não encontrado.");

			context.Todos.Remove(todo);
			context.SaveChanges();

			return Ok(todo);
		}
	}
}