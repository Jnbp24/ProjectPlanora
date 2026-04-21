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
        var createdProjectDto = await _projectService.CreateProjectAsync(projectDTO);
        return CreatedAtAction(nameof(GetProjectByIdAsync), new { projectId = createdProjectDto.ProjectId }, createdProjectDto);
    }
    
    // GET api/project
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProjectDTO>>> GetAllProjectsAsync()
    {
        return Ok(await _projectService.GetAllProjectsAsync());
    }

    // GET api/project/d3eb20c6-2b60-4c82-95e3-b5be7f72cfdc
    [Authorize]
    [HttpGet("{projectId}")]
    public async Task<ActionResult<ProjectDTO>> GetProjectByIdAsync(string projectId)
    {
        return Ok(await _projectService.GetProjectByIdAsync(projectId));
   
    }


    // PUT api/project/d3eb20c6-2b60-4c82-95e3-b5be7f72cfdc
    [Authorize]
    [HttpPut("{projectId}")]
    public async Task<IActionResult> UpdateProjectByIdAsync(string projectId, [FromBody] ProjectDTO projectDTO)
    {
        await _projectService.UpdateProjectByIdAsync(projectId, projectDTO);
        return NoContent();
    }
    // DELETE api/project/d3eb20c6-2b60-4c82-95e3-b5be7f72cfdc
    [Authorize(Roles = "Tovholder")]
    [HttpDelete("{projectId}")]
    public async Task<IActionResult> DeleteProjectByIdAsync(string projectId)
    {
        await _projectService.DeleteProjectByIdAsync(projectId);
        return NoContent();
    }
}