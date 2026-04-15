using Planora.DataAccess.Context;
using Planora.DataAccess.Models;

namespace Planora.DataAccess.Repositories.Project;

public class ProjectRepository : Repository<ProjectDB>, IProjectRepository
{
    public ProjectRepository(DatabaseContext context) : base(context)
    {
        
    }
}