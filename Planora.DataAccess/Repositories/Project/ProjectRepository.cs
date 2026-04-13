using Microsoft.EntityFrameworkCore;
using Planora.DataAccess.Context;
using Planora.DataAccess.Models;

namespace Planora.DataAccess.Repositories.Project;

public class ProjectRepository : IProjectRepository
{
    private readonly DatabaseContext _context;
    private IProjectRepository _projectRepositoryImplementation;

    public ProjectRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Project<ProjectDB> CreateProjectAsync(ProjectDB project)
    {
        _context.Projects.Add(project);
        await _context.SaveChangesAsync();
        return project;
    }

    public Project<ProjectDB> UpdateProjectAsync(string projectId, ProjectDB project)
    {
        throw new NotImplementedException();
        //TODO: Might not need this method, since we can just update the entity and call SaveChangesAsync() in the service layer
        //Check best practice
    }

    public Project<ProjectDB> DeleteProjectAsync(string projectId)
    {
        throw new NotImplementedException();
        //TODO: Might not need this method, since we can just update the entity and call SaveChangesAsync() in the service layer
        //Check best practice
    }

    public async Project<IEnumerable<ProjectDB>> GetAllProjectsAsync()
    {
        return await _context.Projects.ToListAsync();
    }

    public async Project<ProjectDB?> GetProjectByIdAsync(string id)
    {
        return await _context.Projects.FindAsync(Guid.Parse(id));
    }

    public async System.Threading.Projects.Project SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public Project<ProjectDB> AssignTaskToProjectAsync(string projectId, string taskId)
    {
        // Need to create user relation between project and user for this implementation
        throw new NotImplementedException();
    }
}