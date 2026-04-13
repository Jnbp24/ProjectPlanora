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

	public async Task<UserDB> CreateUserAsync(UserDB user)
	{
		_context.Users.Add(user);
		return user;
	}

	public async Task<IEnumerable<UserDB>> GetAllUsersAsync()
	{
		return await _context.Users.ToListAsync();
	}

	public async Task<UserDB?> GetUserByIdAsync(string id)
	{
		return await _context.Users.FindAsync(Guid.Parse(id));
	}

	public async System.Threading.Tasks.Task SaveChangesAsync()
	{
		await _context.SaveChangesAsync();
	}
}