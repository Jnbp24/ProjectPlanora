using Microsoft.AspNetCore.Mvc;
using Planora.DTO.TaskDTO;
using Planora.Api.Services;
using Planora.Api.Services.Task;

namespace Planora.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaskController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TaskController(ITaskService service)
    {
        _taskService = service;
    }

    // POST api/task
    [HttpPost]
    public async Task<ActionResult<TaskDTO>> CreateTaskAsync([FromBody] TaskDTO dto)
    {
        var created = await _taskService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetByIdAsync), new { taskId = created.TaskId }, created);
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


    // PUT api/task/5
    [HttpPut("{taskId:int}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] string taskId, [FromBody] TaskDTO dto)
    {
        var updated = await _taskService.UpdateAsync(taskId, dto);
        return Ok(updated);
    }
    
    // DELETE api/task/5
    [HttpDelete("{taskId:int}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] string taskId)
    {
        await _taskService.DeleteAsync(taskId);
        return NoContent();
    }

    // PUT api/task/5/assign/123
    [HttpPut("{taskId:int}/assign/{categoryName:string}")]
    public async Task<IActionResult> AssignTaskAsync([FromRoute] string taskId, [FromRoute] string categoryName)
    {
        var updatedTask = await _taskService.AssignCategoryByNameAsync(taskId, categoryName);
        return Ok(updatedTask);
    }

    // PUT api/task/5/unassign/123
    [HttpPut("{taskId:int}/unassign/{categoryName:string}")]
    public async Task<IActionResult> UnassignTaskAsync([FromRoute] string taskId, [FromRoute] string categoryName)
    {
        var updatedTask = await _taskService.UnassignCategoryByNameAsync(taskId, categoryName);
        return Ok();
    }

}