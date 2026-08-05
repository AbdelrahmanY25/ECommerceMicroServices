namespace BussinessLogicLayer;

public static class DependencyInjection
{
	extension(IServiceCollection services) 
	{
		public IServiceCollection AddBusinessLogicLayer() 
		{
			services.AddValidatorsFromAssemblyContaining<ProductAddRequestValidator>();

			services.AddScoped<IProductsService, ProductsService>();

			services.AddTransient<IRabbitMQPublisher, RabbitMQPublisher>();

			return services;
		}
	}
}