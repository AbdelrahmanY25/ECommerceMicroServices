namespace Users.Infrastructure.Presistance;

public class AppDbContext
{
	private readonly IConfiguration _configuration;
	private readonly IDbConnection _connection;

	public AppDbContext(IConfiguration configuration)
	{
		_configuration = configuration;

		string connectionStringTemplate = _configuration.GetConnectionString("DefaultConnection")!;
		
		string connectionString = connectionStringTemplate
			.Replace("$POSTGRES_HOST", Environment.GetEnvironmentVariable("POSTGRES_HOST"))
			.Replace("$POSTGRES_PASSWORD", Environment.GetEnvironmentVariable("POSTGRES_PASSWORD"))
			.Replace("$POSTGRES_DATABASE", Environment.GetEnvironmentVariable("POSTGRES_DATABASE"))
			.Replace("$POSTGRES_PORT", Environment.GetEnvironmentVariable("POSTGRES_PORT"))
			.Replace("$POSTGRES_USER", Environment.GetEnvironmentVariable("POSTGRES_USER"));

		_connection = new NpgsqlConnection(connectionString);
	}

	public IDbConnection DbConnection => _connection;
}