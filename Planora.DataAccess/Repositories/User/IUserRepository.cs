namespace Planora.DataAccess.Repositories.User;

public interface IUserRepository
{
	Task<UserDB?> GetUserByIdAsync(string id);
	Task<IEnumerable<UserDB>> GetAllUsersAsync();
	Task<UserDB> CreateUserAsync(UserDB user);
	System.Threading.Tasks.Task SaveChangesAsync();
}