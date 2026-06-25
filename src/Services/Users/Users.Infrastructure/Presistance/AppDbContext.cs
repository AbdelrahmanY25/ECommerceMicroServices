namespace Users.Infrastructure.Presistance;

public class AppDbContext
{
	private readonly IDbConnection _connection;
	private readonly IConfiguration _configuration;

	public AppDbContext(IConfiguration configuration)
	{
		_configuration = configuration;

		string connectionString = _configuration.GetConnectionString("DefaultConnection")!;
				
		_connection = new NpgsqlConnection(connectionString);
	}

	public IDbConnection DbConnection => _connection;
}