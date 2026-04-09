namespace Planora.DataAccess.Repositories.User;

public interface IUserRepository
{
	Task<UserDB?> GetUserById(string id);
	Task<IEnumerable<UserDB>> GetAllUsers();
	Task<UserDB> CreateUser(UserDB user);
	System.Threading.Tasks.Task SaveChanges();
}