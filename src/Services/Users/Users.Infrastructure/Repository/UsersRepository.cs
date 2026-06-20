namespace Users.Infrastructure.Repository;

internal class UsersRepository(AppDbContext dbContext) : IUsersRepository
{
	private readonly AppDbContext _dbContext = dbContext;

	public async Task<ApplicationUser?> AddUser(ApplicationUser user)
	{
		user.UserID = Guid.NewGuid();

		string query = "INSERT INTO public.\"Users\"(\"UserID\", \"Email\", \"PersonName\", \"Gender\", \"Password\") VALUES(@UserID, @Email, @PersonName, @Gender, @Password)";
		int rowCountAffected = await _dbContext.DbConnection.ExecuteAsync(query, user);

		if (rowCountAffected > 0)
		{
			return user;
		}
		else
		{
			return null;
		}
	}

	public async Task<ApplicationUser?> GetUserByEmailAndPassword(string? email, string? password)
	{
		string query = "SELECT * FROM public.\"Users\" WHERE \"Email\"=@Email AND \"Password\"=@Password";
		var parameters = new { Email = email, Password = password };

		ApplicationUser? user = await _dbContext.DbConnection.QueryFirstOrDefaultAsync<ApplicationUser>(query, parameters);

		return user;
	}

	public async Task<ApplicationUser?> GetUserById(Guid? userID)
	{
		var query = "SELECT * FROM public.\"Users\" WHERE \"UserID\" = @UserID";
		var parameters = new { UserID = userID };

		using var connection = _dbContext.DbConnection;
		return await connection.QueryFirstOrDefaultAsync<ApplicationUser>(query, parameters);
	}
}