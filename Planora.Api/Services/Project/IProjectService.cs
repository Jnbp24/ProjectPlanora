using Planora.DTO.ProjectDTO;

namespace Planora.Api.Services.Project;

public interface IProjectService
{
    Task<ProjectDTO> CreateAsync(ProjectDTO dto);
    Task<ProjectDTO> UpdateAsync(string projectId, ProjectDTO dto);
    Task<IEnumerable<ProjectDTO>> GetAllAsync();
    Task<ProjectDTO> GetByIdAsync(string projectId);
    Task<ProjectDTO> DeleteAsync(string projectId);
}