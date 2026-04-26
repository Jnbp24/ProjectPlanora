using System.Data;
using Microsoft.AspNetCore.Identity;
using Planora.Api.Services.Email;
using Planora.DataAccess.Mappers;
using Planora.DataAccess.Models.Auth;
using Planora.DataAccess.Models;
using Planora.DataAccess.Repositories.User;
using Planora.DTO.User;

namespace Planora.Api.Services.User;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly UserManager<AuthUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IEmailService _emailService;
        
    public UserService(IUserRepository repository, UserManager<AuthUser> userManager, IConfiguration configuration, IEmailService emailService)
    {
        _userRepository = repository;
        _userManager = userManager;
        _configuration = configuration;
        _emailService = emailService;
    }
    
    public async Task<UserDTO> CreateUserAsync(UserDTO userDTO)
    {
        //TODO: We should consider making a transaction here, because we need to create an AuthUser and a UserDB, and if one of them fails, we should rollback the other one.
        if(await UserWithEmailExistAsync(userDTO.Email))
        {
            throw new InvalidOperationException("Email already exists");
        }
        
        UserDB userDB = UserMapping.ToEntity(userDTO);
        await _userRepository.CreateAsync(userDB);
        await _userRepository.SaveChangesAsync();
            
        var authUser = new AuthUser
        {
            UserName = userDTO.Email,
            Email = userDTO.Email,
            UserDBId = userDB.UserId,
            UserDb = userDB
        };
        
        var password = _configuration["PasswordManager:userPassword"];

        if (password is null)
            throw new NoNullAllowedException("Loaded password in user is null");
            
        await _userManager.CreateAsync(authUser, password);
        await _emailService.SendSignUpEmailAsync(authUser.Email, password);
            
        return UserMapping.ToDTO(userDB);
    }

    public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
    {
        IEnumerable<UserDB> userDBs = await _userRepository.GetAllAsync();
        return userDBs.Select(UserMapping.ToDTO);
    }

    public async Task<UserDTO> GetUserByIdAsync(string userId)
    {
        if (!Guid.TryParse(userId, out var uGuid))
        {
            throw new ArgumentException($"Invalid userId {userId}");
        }
            
        UserDB userDB = await _userRepository.GetByIdAsync(uGuid);
        return UserMapping.ToDTO(userDB);
    }
        
    public async Task<UserDTO> UpdateUserByIdAsync(string userId, UserDTO userDTO)
    {
        if (!Guid.TryParse(userId, out var uGuid))
        {
            throw new ArgumentException($"Invalid userId {userId}");
        }
            
        UserDB userDB = await _userRepository.GetByIdAsync(uGuid);
          
        userDB.FirstName = userDTO.FirstName;
        userDB.LastName = userDTO.LastName;
        userDB.Tovholder = userDTO.Tovholder;
          
        await _userRepository.SaveChangesAsync();
        return userDTO; 
    }

    public async Task<UserDTO> DeleteUserByIdAsync(string userId)
    {
        if (!Guid.TryParse(userId, out var uGuid))
        {
            throw new ArgumentException($"Invalid userId {userId}");
        }
            
        UserDB deletedUserDB = await _userRepository.GetByIdAsync(uGuid);
        if (deletedUserDB.Deleted)
        {
            throw new NotSupportedException($"{userId} is already deleted");
        }
        deletedUserDB.Deleted = true;
        await _userRepository.SaveChangesAsync();
        return UserMapping.ToDTO(deletedUserDB);
    }

    public async Task<bool> UserWithEmailExistAsync(string email)
    {
        IEnumerable<UserDB> userDBs = await _userRepository.GetAllAsync();
        return userDBs
            .Where(userDB => userDB.Email == email && userDB.Deleted == false)
            .FirstOrDefault() != null;
    }

}