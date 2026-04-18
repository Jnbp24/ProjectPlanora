using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Planora.Api.Services.User;
using Planora.DTO.UserDTO;

namespace Planora.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
	private readonly IUserService _userService;

	public UserController(IUserService service)
	{
		_userService = service;
	}
        
	// To authenticate Tovholder before creating
	// [Authorize(Roles = "Tovholder")]
	// POST api/user
	[Authorize(Roles = "Tovholder")]
	[HttpPost]
	public async Task<ActionResult<UserDTO>> CreateUserAsync(UserDTO userDTO)
	{
		try
		{
			var createdProjectDto = await _userService.CreateUserAsync(userDTO);
			// return 201 with location header pointing to the created resource
			return CreatedAtAction(nameof(GetUserByIdAsync), new { userId = createdProjectDto.UserId }, createdProjectDto);
		}
		catch(InvalidOperationException exception)
		{
			return Conflict(exception.Message);
		}
	}

	// GET api/user
	[Authorize]
	[HttpGet]
	public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsersAsync()
	{
		return Ok(await _userService.GetAllUsersAsync());
	}

	// GET api/user/d3eb20c6-2b60-4c82-95e3-b5be7f72cfdc
	[Authorize]
	[HttpGet]
	[Route("{userId}")]
	public async Task<ActionResult<UserDTO>> GetUserByIdAsync(string userId)
	{
		try
		{
			return Ok(await _userService.GetUserByIdAsync(userId));
		}
		catch (KeyNotFoundException exception)
		{
			return NotFound(exception.Message);
		}
		catch (ArgumentException e)
		{
			return BadRequest(e.Message);
		}
	}

	// To authenticate Tovholder before update
	// [Authorize(Roles = "Tovholder")]
	// PUT api/user/d3eb20c6-2b60-4c82-95e3-b5be7f72cfdc
	[Authorize(Roles = "Tovholder")]
	[HttpPut("{userId}")]
	public async Task<IActionResult> UpdateUserByIdAsync(string userId, [FromBody] UserDTO userDTO)
	{
		try
		{
			return Ok(await _userService.UpdateUserByIdAsync(userId, userDTO));
		}
		catch (KeyNotFoundException e)
		{
			return NotFound(e.Message);
		}
		catch (ArgumentException e)
		{
			return BadRequest(e.Message);
		}
	}

	// DELETE api/user/d3eb20c6-2b60-4c82-95e3-b5be7f72cfdc
	[Authorize]
	[HttpDelete("{userId}")]
	public async Task<IActionResult> DeleteUserById(string userId)
	{
		try
		{
			 await _userService.DeleteUserByIdAsync(userId);
			 return NoContent();
		}
		catch (KeyNotFoundException e)
		{
			return NotFound(e.Message);
		}
		catch (ArgumentException e)
		{
			return BadRequest(e.Message);
		}
	}
        
}