using Planora.DataAccess.Models;

namespace Planora.DataAccess.Repositories.Task;

public interface ITaskRepository : IRepository<TaskDB>
{
    Task<TaskDB> AssignUserToTaskAsync(string taskId, string userId);
    Task<TaskDB> AssignCategoryToTaskByNameAsync(string taskId, string categoryName);
    Task<TaskDB> UnassignCategoryToTaskByNameAsync(string taskId, string categoryName);
}