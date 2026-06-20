namespace DataAccessLayer;

public static class DependencyInjection
{
	extension(IServiceCollection services)
	{
		public IServiceCollection AddDataAccessLayer(IConfiguration configuration)
		{
			string connectionStringTemplate = configuration.GetConnectionString("DefaultConnection")!;

			string connectionString = connectionStringTemplate
					.Replace("$MONGO_HOST", Environment.GetEnvironmentVariable("MONGODB_HOST"))
					.Replace("$MONGO_PORT", Environment.GetEnvironmentVariable("MONGODB_PORT"));

			services.AddSingleton<IMongoClient>(new MongoClient(connectionString));

			services.AddScoped<IMongoDatabase>(provider =>
			{
				IMongoClient client = provider.GetRequiredService<IMongoClient>();
				return client.GetDatabase("Ordersdb");
			});

			services.AddScoped<IOrdersRepository, OrdersRepository>();

			return services;
		}
	}
}