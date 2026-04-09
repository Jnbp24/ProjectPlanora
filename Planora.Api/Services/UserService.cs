using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Planora.Core.DTO;
using Planora.DataAccess.Repositories;

namespace Planora.Api.Services
{
    public class UserService
    {
        private readonly UserRepository _repository;
        
        public UserService(UserRepository repository)
        {
            _repository = repository;
        }

		public async Task<IEnumerable<UserDTO>> GetAllUsers()
		{
			return await _repository.GetAllUsers();
		}

        public async Task<UserDTO> GetUser(string id)
        {
            return await _repository.GetUserById(id);
        }

        public async Task<UserDTO> DeleteUser(string id)
        {
            UserDTO deletedUser = await _repository.DeleteUser(id);
            if (deletedUser == null) {
				throw new KeyNotFoundException($"{id} was not found");
			}
			return deletedUser;
        }

        public async Task<UserDTO> UpdateUser(string id, UserDTO user)
        {
            UserDTO updatedUser = await _repository.UpdateUser(id, user);
			if (updatedUser == null)
			{
				throw new KeyNotFoundException($"{id} was not found");
			}
			return updatedUser; 
		}

    }
}
