using Planora.DataAccess.Models;

namespace Planora.DataAccess.Repositories.Task;

public interface ITaskRepository : IRepository<TaskDB>
{
    Task<TaskDB> AssignUserToTaskAsync(string taskId, string userId);
}