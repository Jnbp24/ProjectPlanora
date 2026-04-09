using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Planora.Core.DTO;
using Planora.DataAccess.Mappers;
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

		public async Task<UserDTO> CreateUser(UserDTO user)
		{
			throw new NotImplementedException();
		}

		public async Task<UserDTO> DeleteUser(string id)
		{
			try
			{
				UserDB userToBeDeleted = await _context.Users.FindAsync(id);
				userToBeDeleted.Deleted = true;
				await _context.SaveChangesAsync();
				return UserMapping.ToDTO(userToBeDeleted);
			}
			catch (NullReferenceException e)
			{
				return null;
			}
		}

		public async Task<IEnumerable<UserDTO>> GetAllUsers()
		{
			IEnumerable<UserDB> userDBs = await _context.Users.ToListAsync();
			return userDBs.Select(UserMapping.ToDTO);
		}

		public async Task<UserDTO?> GetUserById(string id)
		{
			UserDB userDB = await _context.Users.FindAsync(id);
			if(userDB == null)
			{
				return null;
			}
			return UserMapping.ToDTO(userDB);
		}

		public async Task<UserDTO> UpdateUser(string id, UserDTO updatedUser)
		{
			try
			{
				UserDB userToBeUpdated = await _context.Users.FindAsync(id);

				userToBeUpdated.FirstName = updatedUser.FirstName;
				userToBeUpdated.LastName = updatedUser.LastName;
				userToBeUpdated.Tovholder = updatedUser.Tovholder;

				await _context.SaveChangesAsync();
				return updatedUser;
			}
			catch (NullReferenceException e)
			{
				return null;
			}
		}
	}
}
