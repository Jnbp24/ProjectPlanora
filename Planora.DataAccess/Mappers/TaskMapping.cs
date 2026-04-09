using Planora.DataAccess.Models;
using Planora.DTO.TaskDTO;

namespace Planora.DataAccess.Mappers;

internal static class TaskMapping
{
    internal static TaskDB ToEntity(TaskDTO dto)
    {
        return new TaskDB
        {
            TaskId = Guid.NewGuid(),
            Title = dto.Title,
            Content = dto.Content
        };
    }

    internal static TaskDTO ToDTO(TaskDB entity) {
        return new TaskDTO(entity.TaskId.ToString(), entity.Title, entity.Content);
    }
}