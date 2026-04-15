using Microsoft.AspNetCore.Mvc;
using Planora.Api.Services.User;
using Planora.DTO.UserDTO;

namespace Planora.Api.Controllers
{
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
        // POST api/user/d3eb20c6-2b60-4c82-95e3-b5be7f72cfdc
        [HttpPost]
        public async Task<ActionResult<UserDTO>> CreateUserAsync(UserDTO userDTO)
        {
	        try
	        {
		        var createdProjectDto = await _userService.CreateUser(userDTO);
		        // return 201 with location header pointing to the created resource
		        return CreatedAtAction(nameof(GetUserByIdAsync), new { userId = createdProjectDto.UserId }, createdProjectDto);
	        }
	        catch(InvalidOperationException exception)
	        {
		        return StatusCode(500, exception.Message);
	        }
        }

    [Authorize]
		[HttpGet]
		public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsersAsync()
		{
			return Ok(await _userService.GetAllUsers());
		}

		// GET api/user/d3eb20c6-2b60-4c82-95e3-b5be7f72cfdc
		[HttpGet]
		[Route("{userId}")]
        public async Task<ActionResult<UserDTO>> GetUserByIdAsync(string userId)
        {
            try
            {
				return Ok(await _userService.GetUser(userId));
			}
			catch (KeyNotFoundException exception)
            {
				return NotFound(exception.Message);
			}
		}
        
		[HttpGet("{userId}/role")]
		public async Task<ActionResult<string>> GetRoleAsync(string userId)
		{
			throw new NotImplementedException();
		}

		// To authenticate Tovholder before update
		// [Authorize(Roles = "Tovholder")]
		// PUT api/user/d3eb20c6-2b60-4c82-95e3-b5be7f72cfdc
		[HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUserAsync(string userId, UserDTO userDTO)
        {
			try
			{
				return Ok(await _userService.UpdateUser(userId, userDTO));
			}
			catch (KeyNotFoundException e)
			{
				return NotFound(e.Message);
			}
		}

        // DELETE api/user/d3eb20c6-2b60-4c82-95e3-b5be7f72cfdc
		[HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            try
            {
                return Ok(await _userService.DeleteUser(userId));
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
		}
    }
}
