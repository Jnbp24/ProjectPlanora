using Planora.DataAccess.Models;
using System.Threading.Tasks;

namespace Planora.DataAccess.Repositories.Project;

public interface IProjectRepository
{
    Project<ProjectDB?> GetProjectByIdAsync(string projectId);
    Project<IEnumerable<ProjectDB>> GetAllProjectsAsync();
    Project<ProjectDB> CreateProjectAsync(ProjectDB project);
    Project<ProjectDB> UpdateProjectAsync(string projectId, ProjectDB project);
    Project<ProjectDB> DeleteProjectAsync(string projectId);
    Project<ProjectDB> AssignTaskToProjectAsync(string projectId, string taskId);
    System.Threading.Projects.Project SaveChangesAsync();
}