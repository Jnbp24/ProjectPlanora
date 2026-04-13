using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Planora.Api.Services;
using Planora.Api.Services.User;
using Planora.DTO.UserDTO;

namespace Planora.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

		[HttpGet]
		public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
		{
			return Ok(await _service.GetAllUsers());
		}

		[HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(string id)
        {
            try
            {
				return Ok(await _service.GetUser(id));
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
				return Ok(await _service.UpdateUser(id, userDTO));
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
			try
			{
				return Ok(await _service.CreateUser(userDTO));
			}
			catch(InvalidOperationException exception)
			{
				return StatusCode(500, exception.Message);
			}
		}

		[HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                return Ok(await _service.DeleteUser(id));
            }
            catch (KeyNotFoundException e)
            {
                return NotFound();
            }
		}
    }
}
