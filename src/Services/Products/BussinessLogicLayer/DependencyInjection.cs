namespace BussinessLogicLayer;

public static class DependencyInjection
{
	extension(IServiceCollection services) 
	{
		public IServiceCollection AddBusinessLogicLayer() 
		{
			services.AddValidatorsFromAssemblyContaining<ProductAddRequestValidator>();

			services.AddScoped<IProductsService, ProductsService>();

			return services;
		}
	}
}