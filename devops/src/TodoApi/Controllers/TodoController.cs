using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using devops.src.TodoApi.Models;
using devops.src.TodoApi.Services;

namespace devops.src.TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly TodoService _service;

        public TodoController(TodoService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<List<TodoItem>> GetAll() => Ok(_service.GetAll());

        [HttpGet("{id}")]
        public ActionResult<TodoItem> GetById(int id)
        {
            var item = _service.GetById(id);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public ActionResult<TodoItem> Create([FromBody] CreateTodoRequest request)
        {
            try
            {
                var item = _service.Create(request.Title);
                return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}/complete")]
        public ActionResult<TodoItem> Complete(int id)
        {
            var item = _service.Complete(id);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            return _service.Delete(id) ? NoContent() : NotFound();
        }
    }

    public record CreateTodoRequest(string Title);
}

