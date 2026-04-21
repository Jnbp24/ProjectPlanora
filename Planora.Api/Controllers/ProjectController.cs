using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Planora.Api.Services.CalenderYear;
using Planora.DTO.CalenderYearDTO;

namespace Planora.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectController : ControllerBase
{
    private readonly ICalenderYearService _calenderYearService;

    public ProjectController(ICalenderYearService service)
    {
        _calenderYearService = service;
    }

    // POST api/project
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<CalenderYearDTO>> CreateCalenderYearAsync([FromBody] CalenderYearDTO calenderYearDTO)
    {
        var createdCalenderYearDto = await _calenderYearService.CreateCalenderYearAsync(calenderYearDTO);
        return CreatedAtAction(nameof(GetCalenderYearByIdAsync), new { calenderYearId = createdCalenderYearDto.CalenderYearId }, createdCalenderYearDto);
    }
    
    // GET api/project
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CalenderYearDTO>>> GetAllCalenderYearsAsync()
    {
        return Ok(await _calenderYearService.GetAllCalenderYearsAsync());
    }

    // GET api/project/d3eb20c6-2b60-4c82-95e3-b5be7f72cfdc
    [Authorize]
    [HttpGet("{projectId}")]
    public async Task<ActionResult<CalenderYearDTO>> GetCalenderYearByIdAsync(string calenderYearId)
    {
        return Ok(await _calenderYearService.GetCalenderYearByIdAsync(calenderYearId));
   
    }


    // PUT api/project/d3eb20c6-2b60-4c82-95e3-b5be7f72cfdc
    [Authorize]
    [HttpPut("{projectId}")]
    public async Task<IActionResult> UpdateCalenderYearByIdAsync(string calenderYearId, [FromBody] CalenderYearDTO calenderYearDTO)
    {
        await _calenderYearService.UpdateCalenderYearByIdAsync(calenderYearId, calenderYearDTO);
        return NoContent();
    }
    // DELETE api/project/d3eb20c6-2b60-4c82-95e3-b5be7f72cfdc
    [Authorize(Roles = "Tovholder")]
    [HttpDelete("{projectId}")]
    public async Task<IActionResult> DeleteCalenderYearByIdAsync(string calenderYearId)
    {
        await _calenderYearService.DeleteCalenderYearByIdAsync(calenderYearId);
        return NoContent();
    }
}