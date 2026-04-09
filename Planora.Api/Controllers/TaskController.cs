using Microsoft.AspNetCore.Mvc;
using Planora.DTO.TaskDTO;
using Planora.Api.Services;

namespace Planora.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaskController : ControllerBase
{
    private readonly TaskService _taskService;

    public TaskController(TaskService service)
    {
        _taskService = service;
    }

    // POST api/task
    [HttpPost]
    public async Task<ActionResult<TaskDTO>> CreateAsync([FromBody] TaskDTO dto)
    {
        var created = await _taskService.CreateAsync(dto);
        // return 201 with location header pointing to the created resource
        return CreatedAtAction(nameof(GetByIdAsync), new { taskId = /*assumes created has ID, adjust when available*/ 0 }, created);
    }

    // PUT api/task/5
    [HttpPut("{taskId:int}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] string taskId, [FromBody] TaskDTO dto)
    {
        await _taskService.UpdateAsync(taskId, dto);
        return NoContent();
    }
        
    // GET api/task
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskDTO>>> GetAllAsync()
    {
        var items = await _taskService.GetAllAsync();
        return Ok(items);
    }

    // GET api/task/5
    [HttpGet("{taskId:int}")]
    public async Task<ActionResult<TaskDTO>> GetByIdAsync([FromRoute] string taskId)
    {
        var item = await _taskService.GetByIdAsync(taskId);
        if (item is null) return NotFound();
        return Ok(item);
    }

    // DELETE api/task/5
    [HttpDelete("{taskId:int}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] string taskId)
    {
        await _taskService.DeleteAsync(taskId);
        return NoContent();
    }
}