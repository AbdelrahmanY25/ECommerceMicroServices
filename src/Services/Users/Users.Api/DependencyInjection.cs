namespace Users.Api;

public static class DependencyInjection
{
	extension(IServiceCollection services)
	{
		public IServiceCollection AddApiServices(IConfiguration configuration)
		{
			services.AddControllers();

			string connectionString = configuration.GetConnectionString("DefaultConnection")!;
			
			services.AddHealthChecks()
				.AddNpgSql(connectionString, name: "PostgreSQL");

			services.AddCors(options =>
			{
				options.AddDefaultPolicy(builder =>
				{
					builder.AllowAnyOrigin()
						   .AllowAnyMethod()
						   .AllowAnyHeader();
				});
			});

			return services;
		}
	}
}