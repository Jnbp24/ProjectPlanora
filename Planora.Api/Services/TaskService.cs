using Planora.DTO.TaskDTO;
using Planora.DataAccess.Repositories.Task;

namespace Planora.Api.Services;

public class TaskService
{
    private readonly TaskRepository _repo;

    public TaskService(TaskRepository repo)
    {
        _repo = repo;
    }

    public async Task<TaskDTO> CreateAsync(TaskDTO dto) => await _repo.CreateTaskAsync(dto);

    public async Task UpdateAsync(string taskId, TaskDTO dto) => await _repo.UpdateTaskAsync(taskId, dto);
        
    public async Task<IEnumerable<TaskDTO>> GetAllAsync() => await _repo.GetAllTasksAsync();

    public async Task<TaskDTO?> GetByIdAsync(string taskId) => await _repo.GetTaskByIdAsync(taskId);

    public async Task DeleteAsync(string taskId) => await _repo.DeleteTaskAsync(taskId);
}