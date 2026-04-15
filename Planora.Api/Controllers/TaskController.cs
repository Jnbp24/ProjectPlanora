using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Planora.DTO.TaskDTO;
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
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<TaskDTO>> CreateTaskAsync([FromBody] TaskDTO taskDTO)
    {
        var createdTaskDto = await _taskService.CreateAsync(taskDTO);
        // return 201 with location header pointing to the created resource
        return CreatedAtAction(nameof(GetTaskByIdAsync), new { taskId = createdTaskDto.TaskId }, createdTaskDto);
    }
        
    // GET api/task
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskDTO>>> GetAllTasksAsync()
    {
        return Ok(await _taskService.GetAllAsync());
    }

    // GET api/task/d3eb20c6-2b60-4c82-95e3-b5be7f72cfdc
    [Authorize]
    [HttpGet("{taskId}")]
    public async Task<ActionResult<TaskDTO>> GetTaskByIdAsync(string taskId)
    {
        try
        {
            return Ok(await _taskService.GetByIdAsync(taskId));
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }


    // PUT api/task/d3eb20c6-2b60-4c82-95e3-b5be7f72cfdc
    [Authorize]
    [HttpPut("{taskId}")]
    public async Task<IActionResult> UpdateTaskAsync(string taskId, TaskDTO taskDTO)
    {
        try
        {
            return Ok(await _taskService.UpdateAsync(taskId, taskDTO));
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
    
    // DELETE api/task/d3eb20c6-2b60-4c82-95e3-b5be7f72cfdc
    [Authorize]
    [HttpDelete("{taskId}")]
    public async Task<IActionResult> DeleteTaskAsync(string taskId)
    {
        try
        {
            await _taskService.DeleteAsync(taskId);
            return NoContent();
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
    
    // PUT api/task/d3eb20c6-2b60-4c82-95e3-b5be7f72cfdc/category
    [Authorize]
    [HttpPut("{taskId}/category")]
    public async Task<IActionResult> AssignCategoryToTaskAsync(string taskId, [FromBody] string categoryName)
    {
        var updatedTask = await _taskService.AssignCategoryAsync(taskId, categoryName);
        return Ok(updatedTask);
    }

    // DELETE api/task/d3eb20c6-2b60-4c82-95e3-b5be7f72cfdc/category
    [Authorize]
    [HttpDelete("{taskId}/category")]
    public async Task<IActionResult> UnassignCategoryFromTaskAsync(string taskId, [FromBody] string categoryName)
    {
        return Ok(await _taskService.UnassignCategoryAsync(taskId, categoryName));
    }
    
    // POST api/task/d3eb20c6-2b60-4c82-95e3-b5be7f72cfdc/user
    [Authorize]
    [HttpPost("{taskId}/user")]
    public async Task<IActionResult> AssignUserAsync(string taskId, [FromBody] string userId)
    {
        var updatedTask = await _taskService.AssignUserAsync(taskId, userId);
        return Ok(updatedTask);
    }

    // DELETE api/task/d3eb20c6-2b60-4c82-95e3-b5be7f72cfdc/user
    [Authorize]
    [HttpDelete("{taskId}/user")]
    public async Task<IActionResult> UnassignUserAsync(string taskId, [FromBody] string userId)
    {
        var updatedTask = await _taskService.UnassignUserAsync(taskId, userId);
        return Ok(updatedTask);
    }
}