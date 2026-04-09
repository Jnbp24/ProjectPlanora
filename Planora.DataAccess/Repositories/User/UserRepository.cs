using Microsoft.EntityFrameworkCore;
using Planora.DataAccess.Context;

namespace Planora.DataAccess.Repositories.User;

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

	public async System.Threading.Tasks.Task SaveChanges()
	{
		await _context.SaveChangesAsync();
	}
}