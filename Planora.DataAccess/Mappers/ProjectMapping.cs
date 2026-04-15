using Planora.DataAccess.Models;
using Planora.DTO.ProjectDTO;

namespace Planora.DataAccess.Mappers;

public static class ProjectMapping
{
    public static ProjectDB ToEntity(ProjectDTO dto)
    {
        return new ProjectDB
        {
            ProjectId = Guid.NewGuid(),
            Title = dto.Title,
            Content = dto.Content
            };
    }

    public static ProjectDTO ToDTO(ProjectDB entity) {
        return new ProjectDTO(
            ProjectId: entity.ProjectId.ToString(),
            Title: entity.Title,
            Content: entity.Content
            );
    }
}