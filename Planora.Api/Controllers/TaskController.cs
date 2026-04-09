using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Planora.DTO.TaskDTO;
using Planora.Api.Services;

namespace Planora.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly TaskService _service;

        public TaskController(TaskService service)
        {
            _service = service;
        }

        // GET api/task
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskDTO>>> GetAll()
        {
            var items = await _service.GetAllAsync();
            return Ok(items);
        }

        // GET api/task/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<TaskDTO>> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item is null) return NotFound();
            return Ok(item);
        }

        // POST api/task
        [HttpPost]
        public async Task<ActionResult<TaskDTO>> Create([FromBody] TaskDTO dto)
        {
            var created = await _service.CreateAsync(dto);
            // return 201 with location header pointing to the created resource
            return CreatedAtAction(nameof(GetById), new { id = /*assumes created has ID, adjust when available*/ 0 }, created);
        }

        // PUT api/task/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] TaskDTO dto)
        {
            await _service.UpdateAsync(id, dto);
            return NoContent();
        }

        // DELETE api/task/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
