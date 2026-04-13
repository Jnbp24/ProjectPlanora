using Planora.DataAccess.Repositories.Task;
using Planora.DTO.TaskDTO;

namespace Planora.Api.Services.Task;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;

    public TaskService(ITaskRepository repo)
    {
        _taskRepository = repo;
    }

    public async Task<TaskDTO> CreateAsync(TaskDTO dto) => await _taskRepository.CreateTaskAsync(dto);

    public async System.Threading.Tasks.Task UpdateAsync(string taskId, TaskDTO dto) => await _taskRepository.UpdateTaskAsync(taskId, dto);
        
    public async Task<IEnumerable<TaskDTO>> GetAllAsync() => await _taskRepository.GetAllTasksAsync();

    public async Task<TaskDTO?> GetByIdAsync(string taskId) => await _taskRepository.GetTaskByIdAsync(taskId);

    public async System.Threading.Tasks.Task DeleteAsync(string taskId) => await _taskRepository.DeleteTaskAsync(taskId);
}