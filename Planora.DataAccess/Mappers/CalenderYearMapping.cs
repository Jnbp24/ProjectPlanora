using Planora.DataAccess.Models;
using Planora.DTO.CalenderYearDTO;

namespace Planora.DataAccess.Mappers;

public static class CalenderYearMapping
{
    public static CalenderYearDB ToEntity(CalenderYearDTO dto)
    {
        return new CalenderYearDB
        {
            CalenderYearId = Guid.NewGuid(),
            Title = dto.Title,
            Content = dto.Content
            };
    }

    public static CalenderYearDTO ToDTO(CalenderYearDB entity) {
        return new CalenderYearDTO(
            CalenderYearId: entity.CalenderYearId.ToString(),
            Title: entity.Title,
            Content: entity.Content
            );
    }
}