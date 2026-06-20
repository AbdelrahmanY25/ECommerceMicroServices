namespace Users.Infrastructure;

public static class DependencyInjection
{
	extension(IServiceCollection services)
	{
		public IServiceCollection AddInfrastructure()
		{
			services.AddTransient<AppDbContext>();
			
			services.AddTransient<IUsersRepository, UsersRepository>();

			return services;
		}
	}
}