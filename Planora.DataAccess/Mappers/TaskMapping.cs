using Planora.DataAccess.Models;
using Planora.DTO.Task;

namespace Planora.DataAccess.Mappers;

public static class TaskMapping
{
    public static TaskDB ToEntity(TaskDTO dto)
    {
        return new TaskDB
        {
            TaskId = Guid.NewGuid(),
            Title =  dto.Title,
            Content =  dto.Content,
            Deadline = dto.Deadline
        };
    }

    public static TaskDTO ToDTO(TaskDB entity) {
        return new TaskDTO(
            TaskId: entity.TaskId.ToString(), 
            Title: entity.Title, 
            Content: entity.Content,
            Deadline: entity.Deadline
            );
    }
}