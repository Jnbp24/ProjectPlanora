using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Planora.DataAccess.Repositories;
using Planora.DataAccess;
using Planora.DataAccess.Mappers;
using Planora.DataAccess.Repositories.User;
using Planora.DTO.UserDTO;

namespace Planora.Api.Services.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        
        public UserService(IUserRepository repository)
        {
            _userRepository = repository;
        }

		public async Task<IEnumerable<UserDTO>> GetAllUsers()
		{
            IEnumerable<UserDB> userDBs = await _userRepository.GetAllAsync();
            return userDBs.Select(UserMapping.ToDTO);
		}

        public async Task<UserDTO> GetUser(string id)
        {
            UserDB userDB = await _userRepository.GetByIdAsync(id);
            return UserMapping.ToDTO(userDB);
        }
        
        public async Task<UserDTO> UpdateUser(string id, UserDTO userDTO)
        {
            UserDB userDB = await _userRepository.GetByIdAsync(id);
          
            userDB.FirstName = userDTO.FirstName;
            userDB.LastName = userDTO.LastName;
            userDB.Tovholder = userDTO.Tovholder;
          
            _userRepository.SaveChangesAsync();
            return userDTO; 
        }

        public async Task<UserDTO> DeleteUser(string id)
        {
            UserDB deletedUserDB = await _userRepository.GetByIdAsync(id);
            if (deletedUserDB.Deleted)
            {
                throw new NotSupportedException($"{id} is already deleted");
            }
            deletedUserDB.Deleted = true;
            _userRepository.SaveChangesAsync();
            return UserMapping.ToDTO(deletedUserDB);
        }

        public async Task<UserDTO> CreateUser(UserDTO userDTO)
        {
            if(await UserWithEmailExist(userDTO.Email))
            {
                throw new InvalidOperationException("Email already exists");
            }
			UserDB userDB = UserMapping.ToEntity(userDTO);
			await _userRepository.CreateAsync(userDB);
            _userRepository.SaveChangesAsync();
            return UserMapping.ToDTO(userDB);
        }

        public async Task<bool> UserWithEmailExist(string email)
        {
            IEnumerable<UserDB> userDBs = await _userRepository.GetAllAsync();
            return userDBs
                .Where(userDB => userDB.Email == email && userDB.Deleted == false)
                .FirstOrDefault() != null;
		}

    }
}
