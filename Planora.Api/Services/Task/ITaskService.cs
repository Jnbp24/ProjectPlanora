using Planora.DTO.Task;

namespace Planora.Api.Services.Task;

public interface ITaskService
{
    Task<TaskDTO> CreateTaskAsync(TaskDTO taskDTO);
    Task<IEnumerable<TaskDTO>> GetAllTasksAsync();
    Task<TaskDTO?> GetTaskByIdAsync(string taskId);
    Task<TaskDTO> UpdateTaskByIdAsync(string taskId, TaskDTO taskDTO);
    Task<TaskDTO> DeleteTaskByIdAsync(string taskId);
    Task<TaskDTO> AssignCategoryToTaskAsync(string taskId, string categoryName);
    Task<TaskDTO> UnassignCategoryFromTaskAsync(string taskId, string categoryName);
    Task<TaskDTO> AssignUserToTaskAsync(string taskId, string userId);
    Task<TaskDTO> UnassignUserFromTaskAsync(string taskId, string userId);
    Task<IEnumerable<TaskWithCategoryAndUsersDTO>> GetAllTasksIncludeRelationsAsync();
    System.Threading.Tasks.Task MarkTaskAsDoneAsync(string taskId, bool done);
}