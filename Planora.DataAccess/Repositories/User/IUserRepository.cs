using System;
using System.Collections.Generic;
using System.Text;
using Planora.Core.DTO;

namespace Planora.DataAccess.Repositories.User
{
    public interface IUserRepository
    {
		Task<UserDTO?> GetUserById(string id);
		Task<IEnumerable<UserDTO>> GetAllUsers();
		Task<UserDTO> CreateUser(UserDTO user);
		Task<UserDTO> UpdateUser(string id, UserDTO user);
		Task<UserDTO> DeleteUser(string id);
	}
}
