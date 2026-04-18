using Planora.DTO.UserDTO;

namespace Planora.Api.Services.User;

public interface IUserService
{
	Task<IEnumerable<UserDTO>> GetAllUsersAsync();
	Task<UserDTO> GetUserAsync(string userId);
	Task<UserDTO> DeleteUserAsync(string userId);
	Task<UserDTO> UpdateUserAsync(string userId, UserDTO userDTO);
	Task<UserDTO> CreateUserAsync(UserDTO userDTO);

	Task<bool> UserWithEmailExistAsync(string email);

}