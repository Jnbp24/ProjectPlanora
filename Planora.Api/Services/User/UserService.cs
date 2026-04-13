using Planora.DataAccess;
using Planora.DataAccess.Mappers;
using Planora.DataAccess.Repositories.User;
using Planora.DTO.UserDTO;

namespace Planora.Api.Services.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        
        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

		public async Task<IEnumerable<UserDTO>> GetAllUsers()
		{
            IEnumerable<UserDB> userDBs = await _repository.GetAllUsersAsync();
            return userDBs.Select(UserMapping.ToDTO);
		}

        public async Task<UserDTO> GetUser(string id)
        {
            UserDB userDB = await _repository.GetUserByIdAsync(id);
			if(userDB == null)
            {
                throw new KeyNotFoundException();
            }
            return UserMapping.ToDTO(userDB);
        }

        public async Task<UserDTO> DeleteUser(string id)
        {
            UserDB deletedUserDB = await _repository.GetUserByIdAsync(id);
            if (deletedUserDB == null) 
            {
				throw new KeyNotFoundException($"{id} was not found");
			} 
            else if (deletedUserDB.Deleted)
            {
                throw new NotSupportedException($"{id} is already deleted");
            }
            deletedUserDB.Deleted = true;
            await _repository.SaveChangesAsync();
            return UserMapping.ToDTO(deletedUserDB);
        }

        public async Task<UserDTO> UpdateUser(string id, UserDTO userDTO)
        {
            UserDB userDB = await _repository.GetUserByIdAsync(id);
			if (userDB == null)
			{
				throw new KeyNotFoundException($"{id} was not found");
			}
            userDB.FirstName = userDTO.FirstName;
            userDB.LastName = userDTO.LastName;
            userDB.Tovholder = userDTO.Tovholder;
            return userDTO; 
		}

        public async Task<UserDTO> CreateUser(UserDTO userDTO)
        {
            if(await UserWithEmailExist(userDTO))
            {
                throw new InvalidOperationException("Email already exists");
            }
			UserDB userDB = UserMapping.ToEntity(userDTO);
			await _repository.CreateUserAsync(userDB);
            await _repository.SaveChangesAsync();
            return userDTO;
        }

        public async Task<bool> UserWithEmailExist(UserDTO userDTO)
        {
            IEnumerable<UserDB> userDBs = await _repository.GetAllUsersAsync();
            return userDBs
                .Where(userDB => userDB.Email == userDTO.Email && userDB.Deleted == false)
                .FirstOrDefault() != null;
		}

    }
 }
