namespace Users.Api;

public static class DependencyInjection
{
	extension(IServiceCollection services)
	{
		public IServiceCollection AddDependancies(IConfiguration configuration)
		{
			services.AddControllers();

			services
				.AddApplication()
				.AddInfrastructure();

			return services;
		}
	}
}