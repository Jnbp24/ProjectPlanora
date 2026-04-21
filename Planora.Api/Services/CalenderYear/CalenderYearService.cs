using Planora.Api.Services.CalenderYear;
using Planora.DataAccess.Mappers;
using Planora.DataAccess.Repositories.CalenderYear;
using Planora.DTO.CalenderYearDTO;

namespace Planora.Api.Services.CalenderYear;

public class CalenderYearService : ICalenderYearService
{
    private readonly ICalenderYearRepository _calenderYearRepository;

    public CalenderYearService(ICalenderYearRepository calenderYearRepository)
    {
        _calenderYearRepository = calenderYearRepository;
    }

    public async Task<CalenderYearDTO> CreateCalenderYearAsync(CalenderYearDTO calenderYearDTO)
    {
        var calenderYearDB = CalenderYearMapping.ToEntity(calenderYearDTO);
        var createdCalenderYearDB = await _calenderYearRepository.CreateAsync(calenderYearDB);
        return CalenderYearMapping.ToDTO(createdCalenderYearDB);
    }
    
    public async Task<IEnumerable<CalenderYearDTO>> GetAllCalenderYearsAsync()
    {
        var calenderYearDBs = await _calenderYearRepository.GetAllAsync();
        return calenderYearDBs.Select(CalenderYearMapping.ToDTO);
    }

    public async Task<CalenderYearDTO> UpdateCalenderYearByIdAsync(string calenderYearId, CalenderYearDTO calenderYearDTO)
    {
        
        if (!Guid.TryParse(calenderYearId, out var cyGuid))
        {
            throw new ArgumentException($"Invalid projectId {calenderYearId}");
        }
        
        var calenderYearDB = await _calenderYearRepository.GetByIdAsync(cyGuid);
        calenderYearDB.Title = calenderYearDTO.Title;
        calenderYearDB.Content = calenderYearDTO.Content;
        await _calenderYearRepository.SaveChangesAsync();
        return CalenderYearMapping.ToDTO(calenderYearDB);
    }

    public async Task<CalenderYearDTO> GetCalenderYearByIdAsync(string calenderÝearId)
    {
        if (!Guid.TryParse(calenderÝearId, out var cyGuid))
        {
            throw new ArgumentException($"Invalid projectId {calenderÝearId}");
        }
        
        var calenderYearDB = await _calenderYearRepository.GetByIdAsync(cyGuid);
        
        return CalenderYearMapping.ToDTO(calenderYearDB);
    }

    public async Task<CalenderYearDTO> DeleteCalenderYearByIdAsync(string calenderYearId)
    {
        if (!Guid.TryParse(calenderYearId, out var cyGuid))
        {
            throw new ArgumentException($"Invalid projectId {calenderYearId}");
        }
        var calenderYearDB = await _calenderYearRepository.GetByIdAsync(cyGuid);
        
        calenderYearDB.Deleted = true;
        await _calenderYearRepository.SaveChangesAsync();
        return CalenderYearMapping.ToDTO(calenderYearDB);
    }
}