using Planora.DTO.User;

namespace Planora.Api.Services.User;

public interface IUserService
{
	Task<UserDTO> CreateUserAsync(UserDTO userDTO);
	Task<IEnumerable<UserDTO>> GetAllUsersAsync();
	Task<UserDTO> GetUserByIdAsync(string userId);
	Task<UserDTO> UpdateUserByIdAsync(string userId, UserDTO userDTO);
	Task<UserDTO> DeleteUserByIdAsync(string userId);
	Task<bool> UserWithEmailExistAsync(string email);

}