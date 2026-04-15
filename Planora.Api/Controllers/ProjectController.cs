using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Planora.DTO.ProjectDTO;
using Planora.Api.Services.Project;

namespace Planora.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectController(IProjectService service)
    {
        _projectService = service;
    }

    // POST api/project
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ProjectDTO>> CreateProjectAsync([FromBody] ProjectDTO projectDTO)
    {
        var createdProjectDto = await _projectService.CreateAsync(projectDTO);
        // return 201 with location header pointing to the created resource
        return CreatedAtAction(nameof(GetProjectByIdAsync), new { projectId = createdProjectDto.ProjectId }, createdProjectDto);
    }
    
    // GET api/project
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProjectDTO>>> GetAllProjectsAsync()
    {
        return Ok(await _projectService.GetAllAsync());
    }

    // GET api/project/d3eb20c6-2b60-4c82-95e3-b5be7f72cfdc
    [Authorize]
    [HttpGet("{projectId}")]
    public async Task<ActionResult<ProjectDTO>> GetProjectByIdAsync(string projectId)
    {
        try
        {
            return Ok(await _projectService.GetByIdAsync(projectId));
        }
        catch (KeyNotFoundException e)
        {
            return NotFound();
        }
    }


    // PUT api/project/d3eb20c6-2b60-4c82-95e3-b5be7f72cfdc
    [Authorize]
    [HttpPut("{projectId}")]
    public async Task<IActionResult> UpdateProjectAsync(string projectId, [FromBody] ProjectDTO projectDTO)
    {
        try
        {
            return Ok(await _projectService.UpdateAsync(projectId, projectDTO));
        }
        catch (KeyNotFoundException e)
        {
            return NotFound();
        }
        
    }
    // DELETE api/project/d3eb20c6-2b60-4c82-95e3-b5be7f72cfdc
    [Authorize]
    [HttpDelete("{projectId}")]
    public async Task<IActionResult> DeleteProjectAsync(string projectId)
    {
        try
        {
            await _projectService.DeleteAsync(projectId);
            return NoContent();
        }
        catch (KeyNotFoundException e)
        {
            return NotFound();
        }
    }
}