using Planora.DataAccess.Models;
using Planora.DTO.Task;
using Planora.DTO.User;

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

    public static TaskWithCategoryAndUsersDTO ToExtendedDTO(TaskDB entity)
    {
        return new TaskWithCategoryAndUsersDTO(
			TaskId: entity.TaskId.ToString(),
			Title: entity.Title,
			Content: entity.Content,
			Deadline: entity.Deadline,
			Category: CategoryMapping.ToDTO(entity.Category),
	        Users: entity.Users.Select(user => UserMapping.ToDTO(user)).ToList()
		);
    }
}