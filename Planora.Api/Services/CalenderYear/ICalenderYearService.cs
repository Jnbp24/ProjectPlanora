using Planora.DTO.CalenderYearDTO;

namespace Planora.Api.Services.CalenderYear;

public interface ICalenderYearService
{
    Task<CalenderYearDTO> CreateCalenderYearAsync(CalenderYearDTO calenderYearDTO);
    Task<IEnumerable<CalenderYearDTO>> GetAllCalenderYearsAsync();
    Task<CalenderYearDTO> GetCalenderYearByIdAsync(string calenderYearId);
    Task<CalenderYearDTO> UpdateCalenderYearByIdAsync(string calenderYearId, CalenderYearDTO calenderYearDTO);
    Task<CalenderYearDTO> DeleteCalenderYearByIdAsync(string calenderYearId);
}