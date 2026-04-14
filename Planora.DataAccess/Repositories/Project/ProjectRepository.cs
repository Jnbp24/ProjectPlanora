using Microsoft.EntityFrameworkCore;
using Planora.DataAccess.Context;
using Planora.DataAccess.Models;

namespace Planora.DataAccess.Repositories.Project;

public class ProjectRepository : Repository<ProjectDB>, IProjectRepository
{
    private readonly DatabaseContext _context;

    public ProjectRepository(DatabaseContext context) : base(context)
    {
        _context = context;
    }
}