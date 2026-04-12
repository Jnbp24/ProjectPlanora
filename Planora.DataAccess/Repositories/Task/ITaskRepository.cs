using Planora.DataAccess.Models;

namespace Planora.DataAccess.Repositories.Task;

public interface ITaskRepository
{
    Task<TaskDB?> GetTaskByIdAsync(string taskId);
    Task<IEnumerable<TaskDB>> GetAllTasksAsync();
    Task<TaskDB> CreateTaskAsync(TaskDB task);
    Task<TaskDB> UpdateTaskAsync(string taskId, TaskDB task);
    Task<TaskDB> DeleteTaskAsync(string taskId);
    Task<TaskDB> AssignUserToTaskAsync(string taskId, string userId);
    System.Threading.Tasks.Task SaveChangesAsync();
}