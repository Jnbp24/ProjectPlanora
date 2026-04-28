using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Planora.Api.Services.CalenderYear;
using Planora.DTO.CalenderYearDTO;

namespace Planora.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CalenderYearController : ControllerBase
{
    private readonly ICalenderYearService _calenderYearService;

    public CalenderYearController(ICalenderYearService service)
    {
        _calenderYearService = service;
    }

    // POST api/calenderyear
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<CalenderYearDTO>> CreateCalenderYearAsync([FromBody] CalenderYearDTO calenderYearDTO)
    {
        var createdCalenderYearDto = await _calenderYearService.CreateCalenderYearAsync(calenderYearDTO);
        return CreatedAtAction(nameof(GetCalenderYearByIdAsync), new { calenderYearId = createdCalenderYearDto.CalenderYearId }, createdCalenderYearDto);
    }

    // GET api/calenderyear
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CalenderYearDTO>>> GetAllCalenderYearsAsync()
    {
        return Ok(await _calenderYearService.GetAllCalenderYearsAsync());
    }

    // GET api/calenderyear/d3eb20c6-2b60-4c82-95e3-b5be7f72cfdc
    [Authorize]
    [HttpGet("{calenderYearId}")]
    public async Task<ActionResult<CalenderYearDTO>> GetCalenderYearByIdAsync(string calenderYearId)
    {
        return Ok(await _calenderYearService.GetCalenderYearByIdAsync(calenderYearId));
    }

    // PUT api/calenderyear/d3eb20c6-2b60-4c82-95e3-b5be7f72cfdc
    [Authorize]
    [HttpPut("{calenderYearId}")]
    public async Task<IActionResult> UpdateCalenderYearByIdAsync(string calenderYearId, [FromBody] CalenderYearDTO calenderYearDTO)
    {
        await _calenderYearService.UpdateCalenderYearByIdAsync(calenderYearId, calenderYearDTO);
        return NoContent();
    }

    // DELETE api/calenderyear/d3eb20c6-2b60-4c82-95e3-b5be7f72cfdc
    [Authorize(Roles = "Tovholder")]
    [HttpDelete("{calenderYearId}")]
    public async Task<IActionResult> DeleteCalenderYearByIdAsync(string calenderYearId)
    {
        await _calenderYearService.DeleteCalenderYearByIdAsync(calenderYearId);
        return NoContent();
    }
}