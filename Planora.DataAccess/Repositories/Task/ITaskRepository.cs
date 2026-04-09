using Planora.DTO.TaskDTO;

namespace Planora.DataAccess.Repositories.Task;

public interface ITaskRepository
{
    Task<TaskDTO?> GetTaskByIdAsync(string taskId);
    Task<IEnumerable<TaskDTO>> GetAllTasksAsync();
    Task<TaskDTO> CreateTaskAsync(TaskDTO taskDto);
    Task<TaskDTO> UpdateTaskAsync(string taskId, TaskDTO taskDto);
    Task<TaskDTO> DeleteTaskAsync(string taskId);
    Task<TaskDTO> AssignUserToTaskAsync(string taskId, string userId);
}