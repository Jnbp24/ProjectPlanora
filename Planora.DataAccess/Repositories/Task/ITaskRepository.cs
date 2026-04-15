using Planora.DataAccess.Models;

namespace Planora.DataAccess.Repositories.Task;

public interface ITaskRepository : IRepository<TaskDB>
{
    Task<TaskDB> AssignUserAsync(string taskId, string userId);
    Task<TaskDB> UnassignUserAsync(string taskId, string userId);
    Task<TaskDB> AssignCategoryAsync(string taskId, string categoryName);
    Task<TaskDB> UnassignCategoryAsync(string taskId, string categoryName);
}