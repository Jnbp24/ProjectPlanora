using System;
using Planora.DataAccess.Models;
using Planora.DTO.TaskDTO;

namespace Planora.DataAccess.Mappers
{
    internal static class TaskMapping
    {
        internal static TaskDB ToEntity(TaskDTO dto)
        {
            return new TaskDB
            {
                TaskID = Guid.NewGuid(),
                Title = dto.Title,
                Content = dto.Content
            };
        }

        internal static TaskDTO ToDTO(TaskDB entity)
        {
            return new TaskDTO
            {
                Title = entity.Title,
                Content = entity.Content
            };
        }
    }
}
