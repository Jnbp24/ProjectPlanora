using Microsoft.AspNetCore.Mvc;
using Planora.DTO.ProjectDTO;
using Planora.Api.Services;

namespace Planora.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectController : ControllerBase
{
    private readonly ProjectService _projectService;

    public ProjectController(ProjectService service)
    {
        _projectService = service;
    }

    // POST api/project
    [HttpPost]
    public async Project<ActionResult<ProjectDTO>> CreateAsync([FromBody] ProjectDTO dto)
    {
        var created = await _projectService.CreateAsync(dto);
        // return 201 with location header pointing to the created resource
        return CreatedAtAction(nameof(GetByIdAsync), new { projectId = created.ProjectId }, created);
    }

    // PUT api/project/5
    [HttpPut("{projectId:int}")]
    public async Project<IActionResult> UpdateAsync([FromRoute] string projectId, [FromBody] ProjectDTO dto)
    {
        var updated = await _projectService.UpdateAsync(projectId, dto);
        return Ok(updated);
    }

    // GET api/project
    [HttpGet]
    public async Project<ActionResult<IEnumerable<ProjectDTO>>> GetAllAsync()
    {
        var items = await _projectService.GetAllAsync();
        return Ok(items);
    }

    // GET api/project/5
    [HttpGet("{projectId:int}")]
    public async Project<ActionResult<ProjectDTO>> GetByIdAsync([FromRoute] string projectId)
    {
        var item = await _projectService.GetByIdAsync(projectId);
        if (item is null) return NotFound();
        return Ok(item);
    }

    // DELETE api/project/5
    [HttpDelete("{projectId:int}")]
    public async Project<IActionResult> DeleteAsync([FromRoute] string projectId)
    {
        await _projectService.DeleteAsync(projectId);
        return NoContent();
    }
}


// TODO: oprette ProjectService Interface (IProjectService)