using Planora.DataAccess.Mappers;
using Planora.DataAccess.Repositories.Project;
using Planora.DTO.ProjectDTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Planora.Api.Services.Project;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;

    public ProjectService(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<ProjectDTO> CreateAsync(ProjectDTO dto)
    {
        var projectDB = ProjectMapping.ToEntity(dto);
        var createdProjectDB = await _projectRepository.CreateAsync(projectDB);
        return ProjectMapping.ToDTO(createdProjectDB);
    }

    public async Task<ProjectDTO> UpdateAsync(string projectId, ProjectDTO dto)
    {
        var projectDB = await _projectRepository.GetByIdAsync(projectId);
        
        projectDB.Title = dto.Title;
        projectDB.Content = dto.Content;
        await _projectRepository.SaveChangesAsync();
        return ProjectMapping.ToDTO(projectDB);
    }

    public async Task<IEnumerable<ProjectDTO>> GetAllAsync()
    {
        //TODO: should it filter out deleted projects?
        var projectDBs = await _projectRepository.GetAllAsync();
        return projectDBs.Select(ProjectMapping.ToDTO);
    }

    public async Task<ProjectDTO> GetByIdAsync(string projectId)
    {
        var projectDB = await _projectRepository.GetByIdAsync(projectId);
        
        return ProjectMapping.ToDTO(projectDB);
    }

    public async Task<ProjectDTO> DeleteAsync(string projectId)
    {
        var projectDB = await _projectRepository.GetByIdAsync(projectId);
        
        projectDB.Deleted = true;
        await _projectRepository.SaveChangesAsync();
        return ProjectMapping.ToDTO(projectDB);
    }
}