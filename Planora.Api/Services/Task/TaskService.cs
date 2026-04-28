using Planora.DataAccess.Mappers;
using Planora.DataAccess.Models;
using Planora.DataAccess.Repositories.CalenderYear;
using Planora.DataAccess.Repositories.Task;
using Planora.DTO.Task;

namespace Planora.Api.Services.Task;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly ICalenderYearRepository _calenderYearRepository;

    public TaskService(
    ITaskRepository taskRepository,
    ICalenderYearRepository calenderYearRepository)
    {
        _taskRepository = taskRepository;
        _calenderYearRepository = calenderYearRepository;
    }

    public async Task<TaskDTO> CreateTaskAsync(TaskDTO dto)
    {
        var task = TaskMapping.ToEntity(dto);

        var year = dto.Deadline?.Year ?? DateTime.UtcNow.Year;
        
        var calenderYears = await _calenderYearRepository.GetAllAsync();

        var matchingYear = calenderYears
            .FirstOrDefault(cy => cy.Year == year && !cy.Deleted);

        if (matchingYear == null)
        {
            var newYear = new CalenderYearDB
            {
                CalenderYearId = Guid.NewGuid(),
                Title = year.ToString(),
                Year = year
            };

            matchingYear = await _calenderYearRepository.CreateAsync(newYear);
        }

        task.CalenderYearId = matchingYear.CalenderYearId;

        var created = await _taskRepository.CreateAsync(task);

        return TaskMapping.ToDTO(created);
    }

    public async Task<IEnumerable<TaskDTO>> GetAllTasksAsync()
    {
        var taskDBs = await _taskRepository.GetAllAsync();
        
        var filtered = taskDBs.Where(t => !t.Deleted);
        return filtered.Select(TaskMapping.ToDTO);
    }

    public async Task<TaskDTO?> GetTaskByIdAsync(string taskId)
    {
        if (!Guid.TryParse(taskId, out var tGuid))
        {
            throw new ArgumentException($"Invalid taskId {taskId}");
        }
        var taskDB = await _taskRepository.GetByIdAsync(tGuid);
        return TaskMapping.ToDTO(taskDB);
    }

    public async Task<TaskDTO> UpdateTaskByIdAsync(string taskId, TaskDTO taskDTO)
    {
        if (!Guid.TryParse(taskId, out var tGuid))
        {
            throw new ArgumentException($"Invalid taskId {taskId}");
        }
        var taskDB = await _taskRepository.GetByIdAsync(tGuid);
        if (taskDB.Deleted)
        {
            throw new NotSupportedException($"{taskId} is already deleted");
        }
        taskDB.Title = taskDTO.Title;
        taskDB.Content = taskDTO.Content;
        taskDB.Deadline = taskDTO.Deadline;

        if (taskDTO.Deadline.HasValue)
        {
            var year = taskDTO.Deadline.Value.Year;

            var calenderYears = await _calenderYearRepository.GetAllAsync();
            var matchingYear = calenderYears.FirstOrDefault(cy => cy.Year == year && !cy.Deleted);

            if (matchingYear != null)
            {
                taskDB.CalenderYearId = matchingYear.CalenderYearId;
            }
        }
        await _taskRepository.SaveChangesAsync();
        return TaskMapping.ToDTO(taskDB);
    }

    public async Task<TaskDTO> DeleteTaskByIdAsync(string taskId)
    {
        if (!Guid.TryParse(taskId, out var tGuid))
        {
            throw new ArgumentException($"Invalid taskId {taskId}");
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

    public async Task<TaskDTO> AssignCategoryToTaskAsync(string taskId, string categoryName)
    {
        if (!Guid.TryParse(taskId, out var tGuid))
        {
            throw new ArgumentException($"Invalid taskId {taskId}");
        }
        
        var task = await _taskRepository.AssignCategoryAsync(tGuid, categoryName);
        return TaskMapping.ToDTO(task);
    }

    public async Task<TaskDTO> UnassignCategoryFromTaskAsync(string taskId, string categoryName)
    {
        if (!Guid.TryParse(taskId, out var tGuid))
        {
            throw new ArgumentException($"Invalid taskId {taskId}");
        }
        
        var task = await _taskRepository.UnassignCategoryAsync(tGuid, categoryName);
         return TaskMapping.ToDTO(task);
    }

    public async Task<TaskDTO> AssignUserToTaskAsync(string taskId, string userId)
    {
        if (!Guid.TryParse(taskId, out var tGuid))
        {
            throw new ArgumentException($"Invalid taskId {taskId}");
        }
        
        if (!Guid.TryParse(userId, out var uGuid))
            throw new ArgumentException($"Invalid userId {userId}");

        
        var task = await _taskRepository.AssignUserAsync(tGuid, uGuid);
        return TaskMapping.ToDTO(task);
    }

    public async Task<TaskDTO> UnassignUserFromTaskAsync(string taskId, string userId)
    {
        if (!Guid.TryParse(taskId, out var tGuid))
        {
            throw new ArgumentException($"Invalid taskId {taskId}");
        }
        
        if (!Guid.TryParse(userId, out var uGuid))
            throw new ArgumentException($"Invalid userId {userId}");

        
        var task = await _taskRepository.UnassignUserAsync(tGuid, uGuid);
        return TaskMapping.ToDTO(task);
    }

	public async Task<IEnumerable<TaskWithCategoryAndUsersDTO>> GetAllTasksIncludeRelationsAsync()
	{
		var tasksDB = await _taskRepository.GetAllAsync();
		var tasksDTO = tasksDB.Select(taskDB => TaskMapping.ToExtendedDTO(taskDB)).ToList();
		return tasksDTO;
	}
}