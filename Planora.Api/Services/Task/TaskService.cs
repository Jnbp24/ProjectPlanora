using Planora.DataAccess.Mappers;
using Planora.DataAccess.Repositories.Task;
using Planora.DTO.TaskDTO;

namespace Planora.Api.Services.Task;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;

    public TaskService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<TaskDTO> CreateAsync(TaskDTO taskDTO)
    {
        var taskDB = TaskMapping.ToEntity(taskDTO);
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
        if (!Guid.TryParse(taskId, out var tGuid))
        {
            throw new ArgumentException($"Invalid taskId {tGuid}");
        }
        var taskDB = await _taskRepository.GetByIdAsync(tGuid);
        return TaskMapping.ToDTO(taskDB);
    }

    public async Task<TaskDTO> UpdateAsync(string taskId, TaskDTO taskDTO)
    {
        if (!Guid.TryParse(taskId, out var tGuid))
        {
            throw new ArgumentException($"Invalid taskId {tGuid}");
        }
        var taskDB = await _taskRepository.GetByIdAsync(tGuid);
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
        if (!Guid.TryParse(taskId, out var tGuid))
        {
            throw new ArgumentException($"Invalid taskId {tGuid}");
        }
        var taskDB = await _taskRepository.GetByIdAsync(tGuid);
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
        if (!Guid.TryParse(taskId, out var tGuid))
        {
            throw new ArgumentException($"Invalid taskId {tGuid}");
        }
        
        var task = await _taskRepository.AssignCategoryAsync(taskId, categoryName);
        return TaskMapping.ToDTO(task);
    }

    public async Task<TaskDTO> UnassignCategoryAsync(string taskId, string categoryName)
    {
        if (!Guid.TryParse(taskId, out var tGuid))
        {
            throw new ArgumentException($"Invalid taskId {tGuid}");
        }
        
        var task = await _taskRepository.UnassignCategoryAsync(taskId, categoryName);
         return TaskMapping.ToDTO(task);
    }

    public async Task<TaskDTO> AssignUserAsync(string taskId, string userId)
    {
        if (!Guid.TryParse(taskId, out var tGuid))
        {
            throw new ArgumentException($"Invalid taskId {tGuid}");
        }
        
        var task = await _taskRepository.AssignUserAsync(taskId, userId);
        return TaskMapping.ToDTO(task);
    }

    public async Task<TaskDTO> UnassignUserAsync(string taskId, string userId)
    {
        if (!Guid.TryParse(taskId, out var tGuid))
        {
            throw new ArgumentException($"Invalid taskId {tGuid}");
        }
        
        var task = await _taskRepository.UnassignUserAsync(taskId, userId);
        return TaskMapping.ToDTO(task);
    }

}