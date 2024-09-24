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
		public List<TodoModel> Get(
			[FromServices] AppDbContext dbContext
		)
		{
			return dbContext.Todos.ToList();
		}

		[HttpPost("/")]
		public TodoModel Post(
			[FromBody] TodoModel todo,
			[FromServices] AppDbContext dbContext
		)
		{
			dbContext.Todos.Add(todo);
			dbContext.SaveChanges();

			return todo;
		}

		[HttpGet("/{id:int}")]
		public TodoModel GetById(
			[FromRoute] int id,
			[FromServices] AppDbContext context)
		{
			var todo = context.Todos.FirstOrDefault(t => t.Id == id);
			return todo;
		}

		[HttpPut("/{id:int}")]
		public TodoModel Put(
			[FromRoute] int id,
			[FromBody] TodoModel todo,
			[FromServices] AppDbContext context)
		{
			var todoToUpdate = context.Todos.FirstOrDefault(t => t.Id == id);

			todoToUpdate.Title = todo.Title;
			todoToUpdate.Done = todo.Done;
			todoToUpdate.CreatedAt = todo.CreatedAt;

			context.Update(todoToUpdate);
			context.SaveChanges();

			return todoToUpdate;
		}

		[HttpDelete("/{id:int}")]
		public TodoModel Delete(
			[FromRoute] int id,
			[FromServices] AppDbContext context
		)
		{
			var todo = context.Todos.FirstOrDefault(t => t.Id == id);

			context.Remove(todo);
			context.SaveChanges();

			return todo;
		}
	}
}