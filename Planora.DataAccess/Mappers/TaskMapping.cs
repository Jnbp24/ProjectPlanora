using Planora.DataAccess.Models;
using Planora.DTO.TaskDTO;

namespace Planora.DataAccess.Mappers;

public static class TaskMapping
{
    public static TaskDB ToEntity(TaskDTO dto)
    {
        return new TaskDB(
            title: dto.Title,
            content: dto.Content
            );
    }

    public static TaskDTO ToDTO(TaskDB entity) {
        return new TaskDTO(
            TaskId: entity.TaskId.ToString(), 
            Title: entity.Title, 
            Content: entity.Content
            );
    }
}