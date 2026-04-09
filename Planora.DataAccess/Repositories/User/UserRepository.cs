using System;
using System.Collections.Generic;
using System.Text;
using Planora.Core.DTO;
using Planora.DataAccess.Repositories;
using Planora.DataAccess.Repositories.User;

namespace Planora.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseContext _context;
        public UserRepository (DatabaseContext context)
        {
            _context = context;
        }

		public Task<UserDTO> CreateUser(UserDTO user)
		{
			throw new NotImplementedException();
		}

		public Task<UserDTO> DeleteUser(string id)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<UserDTO>> GetAllUsers()
		{
			throw new NotImplementedException();
		}

		public Task<UserDTO?> GetUserById(string id)
		{
			throw new NotImplementedException();
		}

		public Task<UserDTO> UpdateUser(string id, UserDTO user)
		{
			throw new NotImplementedException();
		}
	}
}
