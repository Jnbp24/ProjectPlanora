using Microsoft.AspNetCore.Mvc;
using Planora.Api.Services;
using Planora.DTO.UserDTO;

namespace Planora.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService service)
        {
            _userService = service;
        }

		[HttpGet]
		public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
		{
			return Ok(await _userService.GetAllUsers());
		}

		[HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(string id)
        {
            try
            {
				return Ok(await _userService.GetUser(id));
			}
			catch (KeyNotFoundException)
            {
				return NotFound();
			}
		}

		[HttpGet]
		public async Task<ActionResult<string>> GetRole()
		{
			throw new NotImplementedException();
		}

		// To authenticate Tovholder before update
		// [Authorize(Roles = "Tovholder")]
		[HttpPut("{id}")]
        public async Task<IActionResult> PutUser(string id, UserDTO userDTO)
        {
			try
			{
				return Ok(await _userService.UpdateUser(id, userDTO));
			}
			catch (KeyNotFoundException e)
			{
				return NotFound();
			}
		}

		// To authenticate Tovholder before creating
        // [Authorize(Roles = "Tovholder")]
		[HttpPost]
        public async Task<ActionResult<UserDTO>> PostUser(UserDTO userDTO)
        {
			throw new NotImplementedException();
		}

		[HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                return Ok(await _userService.DeleteUser(id));
            }
            catch (KeyNotFoundException e)
            {
                return NotFound();
            }
		}
    }
}
