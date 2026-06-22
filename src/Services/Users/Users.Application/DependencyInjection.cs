namespace Users.Application;

public static class DependencyInjection
{
	extension(IServiceCollection services) 
	{
		public IServiceCollection AddApplicationServices()
		{
			services.AddTransient<IUsersService, UsersService>();

			return services;
		}
	}
}