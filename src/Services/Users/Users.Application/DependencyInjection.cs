namespace Users.Application;

public static class DependencyInjection
{
	extension(IServiceCollection services) 
	{
		public IServiceCollection AddApplication()
		{
			services.AddTransient<IUsersService, UsersService>();

			return services;
		}
	}
}