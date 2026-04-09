using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Planora.Api.Services;
using Planora.Core.DTO;

namespace Planora.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _service;

        public UserController(UserService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUserDB(Guid id)
        {
			throw new NotImplementedException();
		}

		[HttpPut("{id}")]
        public async Task<IActionResult> PutUserDB(Guid id, UserDTO userDB)
        {
			throw new NotImplementedException();
		}

		[HttpPost]
        public async Task<ActionResult<UserDTO>> PostUserDB(UserDTO userDB)
        {
			throw new NotImplementedException();
		}

		[HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserDB(Guid id)
        {
			throw new NotImplementedException();
		}
    }
}
