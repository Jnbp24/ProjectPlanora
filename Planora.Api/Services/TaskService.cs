using Planora.DTO.TaskDTO;
using Planora.DataAccess.Repositories.Task;
using Planora.DataAccess.Mappers;

namespace Planora.Api.Services;

public class TaskService
{
    private readonly ITaskRepository _taskRepository;

    public TaskService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<TaskDTO> CreateAsync(TaskDTO dto)
    {
        var taskDB = TaskMapping.ToEntity(dto);
        var createdTaskDB = await _taskRepository.CreateTaskAsync(taskDB);
        return TaskMapping.ToDTO(createdTaskDB);
    }

    public async Task<TaskDTO> UpdateAsync(string taskId, TaskDTO dto)
    {
        var taskDB = await _taskRepository.GetTaskByIdAsync(taskId);
        if (taskDB == null)
        {
            throw new KeyNotFoundException($"Task {taskId} not found");
        }
        taskDB.Title = dto.Title;
        taskDB.Content = dto.Content;
        await _taskRepository.SaveChangesAsync();
        return TaskMapping.ToDTO(taskDB);
    }
        
    public async Task<IEnumerable<TaskDTO>> GetAllAsync()
    {
        //TODO: should it filter out deleted tasks?
        var taskDBs = await _taskRepository.GetAllTasksAsync();
        return taskDBs.Select(TaskMapping.ToDTO);
    }

    public async Task<TaskDTO> GetByIdAsync(string taskId)
    {
        var taskDB = await _taskRepository.GetTaskByIdAsync(taskId);
        if (taskDB == null)
        {
            throw new KeyNotFoundException($"Task {taskId} not found");
        }
        return TaskMapping.ToDTO(taskDB);
    }

    public async Task<TaskDTO> DeleteAsync(string taskId)
    {
        var taskDB = await _taskRepository.GetTaskByIdAsync(taskId);
        if (taskDB == null)
        {
            throw new KeyNotFoundException($"Task {taskId} not found");
        }
        taskDB.Deleted = true;
        await _taskRepository.SaveChangesAsync();
        return TaskMapping.ToDTO(taskDB);
    }
}