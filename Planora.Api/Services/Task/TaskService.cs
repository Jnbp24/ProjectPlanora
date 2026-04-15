using Planora.Api.Services.Task;
using Planora.DTO.TaskDTO;
using Planora.DataAccess.Repositories.Task;
using Planora.DataAccess.Mappers;

namespace Planora.Api.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;

    public TaskService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<TaskDTO> CreateAsync(TaskDTO dto)
    {
        var taskDB = TaskMapping.ToEntity(dto);
        var createdTaskDB = await _taskRepository.CreateAsync(taskDB);
        return TaskMapping.ToDTO(createdTaskDB);
    }
        
    public async Task<IEnumerable<TaskDTO>> GetAllAsync()
    {
        var taskDBs = await _taskRepository.GetAllAsync();


        var filtered = taskDBs.Where(t => !t.Deleted);
        return filtered.Select(TaskMapping.ToDTO);
    }

    public async Task<TaskDTO> GetByIdAsync(string taskId)
    {
        var taskDB = await _taskRepository.GetByIdAsync(taskId);
        return TaskMapping.ToDTO(taskDB);
    }

    public async Task<TaskDTO> UpdateAsync(string taskId, TaskDTO dto)
    {
        var taskDB = await _taskRepository.GetByIdAsync(taskId);
        if (taskDB.Deleted)
        {
            throw new NotSupportedException($"{taskId} is already deleted");
        }
        taskDB.Title = dto.Title;
        taskDB.Content = dto.Content;
        await _taskRepository.SaveChangesAsync();
        return TaskMapping.ToDTO(taskDB);
    }

    public async Task<TaskDTO> DeleteAsync(string taskId)
    {
        var taskDB = await _taskRepository.GetByIdAsync(taskId);
        if (taskDB.Deleted)
        {
            throw new NotSupportedException($"{taskId} is already deleted");
        }
        taskDB.Deleted = true;
        await _taskRepository.SaveChangesAsync();
        return TaskMapping.ToDTO(taskDB);
    }

    public async Task<TaskDTO> AssignCategoryByNameAsync(string taskId, string categoryName)
    {
        var task = await _taskRepository.AssignCategoryToTaskByNameAsync(taskId, categoryName);
        return TaskMapping.ToDTO(task);
    }

    public async Task<TaskDTO> UnassignCategoryByNameAsync(string taskId, string categoryName)
    {
        var task = await _taskRepository.UnassignCategoryToTaskByNameAsync(taskId, categoryName);
         return TaskMapping.ToDTO(task);
    }

    public async Task<TaskDTO> AssignUserToTaskAsync(string taskId, string userId)
    {
        var task = await _taskRepository.AssignUserToTaskAsync(taskId, userId);
        return TaskMapping.ToDTO(task);
    }

    public async Task<TaskDTO> UnassignUserFromTaskAsync(string taskId, string userId)
    {
        var task = await _taskRepository.UnassignUserFromTaskAsync(taskId, userId);
        return TaskMapping.ToDTO(task);
    }

}