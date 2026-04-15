using Planora.DataAccess.Context;
using Planora.DataAccess.Models;

namespace Planora.DataAccess.Repositories.User;

public class UserRepository : Repository<UserDB>, IUserRepository
{
	public UserRepository (DatabaseContext context) : base(context)
	{
	}
}