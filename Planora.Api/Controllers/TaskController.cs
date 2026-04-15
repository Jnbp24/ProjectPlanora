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
    [HttpPost]
    public async Task<ActionResult<TaskDTO>> CreateTaskAsync([FromBody] TaskDTO taskDTO)
    {
        var createdTaskDto = await _taskService.CreateAsync(taskDTO);
        // return 201 with location header pointing to the created resource
        return CreatedAtAction(nameof(GetTaskByIdAsync), new { taskId = createdTaskDto.TaskId }, createdTaskDto);
    }
        
    // GET api/task
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskDTO>>> GetAllTasksAsync()
    {
        return Ok(await _taskService.GetAllAsync());
    }

    // GET api/task/d3eb20c6-2b60-4c82-95e3-b5be7f72cfdc
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
    
    // ALL CODE BELOW THIS HAS NOT BEEN TESTED PROPERLY
    
    // PUT api/task/5/assign/123
    [HttpPut("{taskId}/assign/{categoryName}")]
    public async Task<IActionResult> AssignTaskAsync(string taskId, string categoryName)
    {
        var updatedTask = await _taskService.AssignCategoryByNameAsync(taskId, categoryName);
        return Ok(updatedTask);
    }

    // PUT api/task/5/unassign/123
    [HttpPut("{taskId}/unassign/{categoryName}")]
    public async Task<IActionResult> UnassignTaskAsync(string taskId, string categoryName)
    {
        var updatedTask = await _taskService.UnassignCategoryByNameAsync(taskId, categoryName);
        return Ok(updatedTask);
    }
    
    // PUT api/task/5/assign/123
    [HttpPut("{taskId}/assignUser/{userId}")]
    public async Task<IActionResult> AssignUserToTaskAsync(string taskId, string userId)
    {
        var updatedTask = await _taskService.AssignUserToTaskAsync(taskId, userId);
        return Ok(updatedTask);
    }

    [HttpPut("{taskId}/unassignUser/{userId}")]
    public async Task<IActionResult> UnassignUserFromTaskAsync(string taskId, string userId)
    {
        var updatedTask = await _taskService.UnassignUserFromTaskAsync(taskId, userId);
        return Ok(updatedTask);
    }
}