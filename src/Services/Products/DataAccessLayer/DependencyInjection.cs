namespace DataAccessLayer;

public static class DependencyInjection
{
	extension(IServiceCollection services)
	{
		public IServiceCollection AddDataAccessLayer(IConfiguration configuration)
		{
			services.AddDbContext<ApplicationDbContext>(options => {
				options.UseMySQL(configuration.GetConnectionString("DefaultConnection")!);
			});

			services.AddScoped<IProductsRepository, ProductsRepository>();

			return services;
		}
	}
}