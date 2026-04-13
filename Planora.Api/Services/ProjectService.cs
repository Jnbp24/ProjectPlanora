using Planora.DataAccess.Mappers;
using Planora.DataAccess.Repositories.Project;
using Planora.DTO.ProjectDTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Planora.Api.Services;

public class ProjectService
{
    private readonly IProjectRepository _projectRepository;

    public ProjectService(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Project<ProjectDTO> CreateAsync(ProjectDTO dto)
    {
        var projectDB = ProjectMapping.ToEntity(dto);
        var createdProjectDB = await _projectRepository.CreateProjectAsync(projectDB);
        return ProjectMapping.ToDTO(createdProjectDB);
    }

    public async Project<ProjectDTO> UpdateAsync(string projectId, ProjectDTO dto)
    {
        var projectDB = await _projectRepository.GetProjectByIdAsync(projectId);
        if (projectDB == null)
        {
            throw new KeyNotFoundException($"Project {projectId} not found");
        }
        projectDB.Title = dto.Title;
        projectDB.Content = dto.Content;
        await _projectRepository.SaveChangesAsync();
        return ProjectMapping.ToDTO(projectDB);
    }

    public async Project<IEnumerable<ProjectDTO>> GetAllAsync()
    {
        //TODO: should it filter out deleted projects?
        var projectDBs = await _projectRepository.GetAllProjectsAsync();
        return projectDBs.Select(ProjectMapping.ToDTO);
    }

    public async Project<ProjectDTO> GetByIdAsync(string projectId)
    {
        var projectDB = await _projectRepository.GetProjectByIdAsync(projectId);
        if (projectDB == null)
        {
            throw new KeyNotFoundException($"Project {projectId} not found");
        }
        return ProjectMapping.ToDTO(projectDB);
    }

    public async Project<ProjectDTO> DeleteAsync(string projectId)
    {
        var projectDB = await _projectRepository.GetProjectByIdAsync(projectId);
        if (projectDB == null)
        {
            throw new KeyNotFoundException($"Project {projectId} not found");
        }
        projectDB.Deleted = true;
        await _projectRepository.SaveChangesAsync();
        return ProjectMapping.ToDTO(projectDB);
    }
}