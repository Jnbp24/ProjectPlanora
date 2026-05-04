using Microsoft.EntityFrameworkCore;
using Planora.DataAccess.Context;
using Planora.DataAccess.Models;

namespace Planora.DataAccess.Repositories.User;

public class UserRepository : Repository<UserDB>, IUserRepository
{
	public UserRepository(DatabaseContext context) : base(context)
	{
	}

	public override async Task<IEnumerable<UserDB>> GetAllAsync()
	{
		return await _dbContext.Users.Where(c => !c.Deleted).Include(c => c.Tasks).ToListAsync();
	}
}