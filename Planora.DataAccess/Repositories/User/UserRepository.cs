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

		public async Task<UserDB> CreateUser(UserDB user)
		{
			throw new NotImplementedException();
		}

		public async Task<IEnumerable<UserDB>> GetAllUsers()
		{
			return await _context.Users.ToListAsync();
		}

		public async Task<UserDB?> GetUserById(string id)
		{
			return await _context.Users.FindAsync(id);
		}

		public async Task SaveChanges()
		{
			await _context.SaveChangesAsync();
	
		}
	}
}
