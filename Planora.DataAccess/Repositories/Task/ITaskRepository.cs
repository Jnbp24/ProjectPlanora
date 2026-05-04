using Planora.DataAccess.Models;

namespace Planora.DataAccess.Repositories.Task;

public interface ITaskRepository : IRepository<TaskDB>
{
    Task<TaskDB> AssignUserAsync(Guid taskId, Guid userId);
    Task<TaskDB> UnassignUserAsync(Guid taskId, Guid userId);
    Task<TaskDB> AssignCategoryAsync(Guid taskId, string categoryName);
    Task<TaskDB> UnassignCategoryAsync(Guid taskId, string categoryName);
    System.Threading.Tasks.Task MarkTaskAsDoneAsync(Guid taskId, bool done);
}