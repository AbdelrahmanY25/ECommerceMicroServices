namespace Users.Api;

public static class DependencyInjection
{
	extension(IServiceCollection services)
	{
		public IServiceCollection AddApiServices(IConfiguration configuration)
		{
			services.AddControllers();

			string connectionStringTemplate = configuration.GetConnectionString("DefaultConnection")!;

			string connectionString = connectionStringTemplate
				.Replace("$POSTGRES_HOST", Environment.GetEnvironmentVariable("POSTGRES_HOST"))
				.Replace("$POSTGRES_PASSWORD", Environment.GetEnvironmentVariable("POSTGRES_PASSWORD"))
				.Replace("$POSTGRES_DATABASE", Environment.GetEnvironmentVariable("POSTGRES_DATABASE"))
				.Replace("$POSTGRES_PORT", Environment.GetEnvironmentVariable("POSTGRES_PORT"))
				.Replace("$POSTGRES_USER", Environment.GetEnvironmentVariable("POSTGRES_USER"));

			services.AddHealthChecks()
				.AddNpgSql(connectionString, name: "PostgreSQL");

			return services;
		}
	}
}