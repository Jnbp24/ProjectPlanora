using Planora.DTO.ProjectDTO;

namespace Planora.Api.Services.Project;

public interface IProjectService
{
    Task<ProjectDTO> CreateProjectAsync(ProjectDTO projectDTO);
    Task<IEnumerable<ProjectDTO>> GetAllProjectsAsync();
    Task<ProjectDTO> GetProjectByIdAsync(string projectId);
    Task<ProjectDTO> UpdateProjectByIdAsync(string projectId, ProjectDTO projectDTO);
    Task<ProjectDTO> DeleteProjectByIdAsync(string projectId);
}