using Planora.DTO.TaskDTO;
using Planora.DataAccess.Repositories.Task;
using Planora.DataAccess.Mappers;

namespace Planora.Api.Services.Task;

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
		//TODO: should it filter out deleted tasks?
		var taskDBs = await _taskRepository.GetAllAsync();
		return taskDBs.Select(TaskMapping.ToDTO);
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
		_taskRepository.SaveChangesAsync();
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
		_taskRepository.SaveChangesAsync();
		return TaskMapping.ToDTO(taskDB);
	}
}