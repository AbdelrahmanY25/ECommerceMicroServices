namespace Users.Application.IRepository;

public interface IUsersRepository
{
	Task<ApplicationUser?> AddUser(ApplicationUser user);
	Task<ApplicationUser?> GetUserByEmailAndPassword(string? email, string? password);
}