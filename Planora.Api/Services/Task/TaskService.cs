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
        taskDB.Deadline = dto.Deadline;
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

    public async Task<TaskDTO> AssignCategoryAsync(string taskId, string categoryName)
    {
        var task = await _taskRepository.AssignCategoryAsync(taskId, categoryName);
        return TaskMapping.ToDTO(task);
    }

    public async Task<TaskDTO> UnassignCategoryAsync(string taskId, string categoryName)
    {
        var task = await _taskRepository.UnassignCategoryAsync(taskId, categoryName);
         return TaskMapping.ToDTO(task);
    }

    public async Task<TaskDTO> AssignUserAsync(string taskId, string userId)
    {
        var task = await _taskRepository.AssignUserAsync(taskId, userId);
        return TaskMapping.ToDTO(task);
    }

    public async Task<TaskDTO> UnassignUserAsync(string taskId, string userId)
    {
        var task = await _taskRepository.UnassignUserAsync(taskId, userId);
        return TaskMapping.ToDTO(task);
    }

}