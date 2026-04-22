using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Planora.Api.Services.User;
using Planora.DTO.User;

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
			return CreatedAtAction(nameof(GetUserByIdAsync), new { userId = createdProjectDto.UserId }, createdProjectDto);
		}
		catch (InvalidOperationException exception)
		{
			return Conflict(new { message = exception.Message });
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
		return Ok(await _userService.GetUserByIdAsync(userId));
	}

	// To authenticate Tovholder before update
	// [Authorize(Roles = "Tovholder")]
	// PUT api/user/d3eb20c6-2b60-4c82-95e3-b5be7f72cfdc
	[Authorize(Roles = "Tovholder")]
	[HttpPut("{userId}")]
	public async Task<IActionResult> UpdateUserByIdAsync(string userId, [FromBody] UserDTO userDTO)
	{
		return Ok(await _userService.UpdateUserByIdAsync(userId, userDTO));
	
	}

	// DELETE api/user/d3eb20c6-2b60-4c82-95e3-b5be7f72cfdc
	[Authorize(Roles = "Tovholder")]
	[HttpDelete("{userId}")]
	public async Task<IActionResult> DeleteUserById(string userId)
	{
			 await _userService.DeleteUserByIdAsync(userId);
			 return NoContent();
	}
}