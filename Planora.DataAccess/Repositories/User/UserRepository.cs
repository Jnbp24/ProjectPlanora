using Planora.DataAccess.Context;

namespace Planora.DataAccess.Repositories.User;

public class UserRepository : Repository<UserDB>, IUserRepository
{
	public UserRepository (DatabaseContext context) : base(context)
	{
	}
}