using System;
using System.Collections.Generic;
using System.Text;
using Planora.Core.DTO;

namespace Planora.DataAccess.Repositories.User
{
    public interface IUserRepository
    {
		Task<UserDB?> GetUserById(string id);
		Task<IEnumerable<UserDB>> GetAllUsers();
		Task<UserDB> CreateUser(UserDB user);
		Task SaveChanges();
	}
}
