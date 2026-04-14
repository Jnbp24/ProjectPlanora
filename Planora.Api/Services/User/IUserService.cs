using System.Collections.Generic;
using System.Threading.Tasks;
using Planora.DTO.UserDTO;

namespace Planora.Api.Services.User
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetAllUsers();
        Task<UserDTO> GetUser(string id);
        Task<UserDTO> DeleteUser(string id);
        Task<UserDTO> UpdateUser(string id, UserDTO userDTO);
        Task<UserDTO> CreateUser(UserDTO userDTO);

		Task<bool> UserWithEmailExist(string email);

	}
}
