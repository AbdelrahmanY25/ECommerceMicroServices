namespace Users.Infrastructure;

public static class DependencyInjection
{
	extension(IServiceCollection services)
	{
		public IServiceCollection AddInfrastructureServices()
		{
			services.AddTransient<AppDbContext>();
			
			services.AddTransient<IUsersRepository, UsersRepository>();

			return services;
		}
	}
}