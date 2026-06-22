namespace DataAccessLayer;

public static class DependencyInjection
{
	extension(IServiceCollection services)
	{
		public IServiceCollection AddDataAccessLayer(IConfiguration configuration)
		{
			string connectionString = configuration.GetConnectionString("DefaultConnection")!;

			services.AddSingleton<IMongoClient>(new MongoClient(connectionString));

			services.AddScoped<IMongoDatabase>(provider =>
			{
				IMongoClient client = provider.GetRequiredService<IMongoClient>();
				return client.GetDatabase("ordersDb");
			});

			services.AddScoped<IOrdersRepository, OrdersRepository>();

			return services;
		}
	}
}