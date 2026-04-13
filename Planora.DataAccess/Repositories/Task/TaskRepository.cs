using Planora.DataAccess.Context;
using Planora.DataAccess.Models;

namespace Planora.DataAccess.Repositories.Task;

public class TaskRepository : Repository<TaskDB>, ITaskRepository
{

    public TaskRepository(DatabaseContext context) : base(context)
    {
    }
    
    public Task<TaskDB> AssignUserToTaskAsync(string taskId, string userId)
    {
        // Need to create user relation between task and user for this implementation
        throw new NotImplementedException();
    }
}