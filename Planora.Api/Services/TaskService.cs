using Planora.DTO.TaskDTO;
using Planora.DataAccess.Repositories.Task;

namespace Planora.Api.Services;

public class TaskService
{
    private readonly TaskRepository _taskRepository;

    public TaskService(TaskRepository repo)
    {
        _taskRepository = repo;
    }

    public async Task<TaskDTO> CreateAsync(TaskDTO dto) => await _taskRepository.CreateTaskAsync(dto);

    public async Task UpdateAsync(string taskId, TaskDTO dto) => await _taskRepository.UpdateTaskAsync(taskId, dto);
        
    public async Task<IEnumerable<TaskDTO>> GetAllAsync() => await _taskRepository.GetAllTasksAsync();

    public async Task<TaskDTO?> GetByIdAsync(string taskId) => await _taskRepository.GetTaskByIdAsync(taskId);

    public async Task DeleteAsync(string taskId) => await _taskRepository.DeleteTaskAsync(taskId);
}