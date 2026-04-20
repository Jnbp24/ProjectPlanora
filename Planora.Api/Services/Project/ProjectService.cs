using Planora.DataAccess.Mappers;
using Planora.DataAccess.Repositories.Project;
using Planora.DTO.ProjectDTO;

namespace Planora.Api.Services.Project;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;

    public ProjectService(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<ProjectDTO> CreateProjectAsync(ProjectDTO projectDTO)
    {
        var projectDB = ProjectMapping.ToEntity(projectDTO);
        var createdProjectDB = await _projectRepository.CreateAsync(projectDB);
        return ProjectMapping.ToDTO(createdProjectDB);
    }
    
    public async Task<IEnumerable<ProjectDTO>> GetAllProjectsAsync()
    {
        //TODO: should it filter out deleted projects?
        var projectDBs = await _projectRepository.GetAllAsync();
        return projectDBs.Select(ProjectMapping.ToDTO);
    }

    public async Task<ProjectDTO> UpdateProjectByIdAsync(string projectId, ProjectDTO projectDTO)
    {
        
        if (!Guid.TryParse(projectId, out var pGuid))
        {
            throw new ArgumentException($"Invalid projectId {projectId}");
        }
        
        var projectDB = await _projectRepository.GetByIdAsync(pGuid);
        projectDB.Title = projectDTO.Title;
        projectDB.Content = projectDTO.Content;
        await _projectRepository.SaveChangesAsync();
        return ProjectMapping.ToDTO(projectDB);
    }

    public async Task<ProjectDTO> GetProjectByIdAsync(string projectId)
    {
        if (!Guid.TryParse(projectId, out var pGuid))
        {
            throw new ArgumentException($"Invalid projectId {projectId}");
        }
        
        var projectDB = await _projectRepository.GetByIdAsync(pGuid);
        
        return ProjectMapping.ToDTO(projectDB);
    }

    public async Task<ProjectDTO> DeleteProjectByIdAsync(string projectId)
    {
        if (!Guid.TryParse(projectId, out var pGuid))
        {
            throw new ArgumentException($"Invalid projectId {projectId}");
        }
        var projectDB = await _projectRepository.GetByIdAsync(pGuid);
        
        projectDB.Deleted = true;
        await _projectRepository.SaveChangesAsync();
        return ProjectMapping.ToDTO(projectDB);
    }
}